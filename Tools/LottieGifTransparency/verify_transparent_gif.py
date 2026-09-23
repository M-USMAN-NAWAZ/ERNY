#!/usr/bin/env python3
"""Verify GIF timing and transparency against a Lottie RGBA atlas."""

from __future__ import annotations

import argparse
import json
from pathlib import Path

from PIL import Image, ImageChops

from convert_lottie_atlas_to_transparent_gif import frame_durations, gif_frame


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser()
    parser.add_argument("--lottie", required=True, type=Path)
    parser.add_argument("--sheet", required=True, type=Path)
    parser.add_argument("--gif", required=True, type=Path)
    parser.add_argument("--report", required=True, type=Path)
    parser.add_argument("--columns", type=int, default=11)
    parser.add_argument("--rows", type=int, default=11)
    parser.add_argument("--alpha-threshold", type=int, default=127)
    return parser.parse_args()


def main() -> None:
    args = parse_args()
    metadata = json.loads(args.lottie.read_text(encoding="utf-8"))
    width = int(metadata["w"])
    height = int(metadata["h"])
    fps = float(metadata["fr"])
    source_frame_count = int(float(metadata["op"]) - float(metadata["ip"]))
    durations = frame_durations(source_frame_count, fps)

    sheet = Image.open(args.sheet).convert("RGBA")
    cell_width = sheet.width // args.columns
    cell_height = sheet.height // args.rows
    expected_frames: list[Image.Image] = []
    expected_durations: list[int] = []
    previous_bytes: bytes | None = None

    for index in range(source_frame_count):
        column = index % args.columns
        row = index // args.columns
        rgba = sheet.crop(
            (
                column * cell_width,
                row * cell_height,
                (column + 1) * cell_width,
                (row + 1) * cell_height,
            )
        ).resize((width, height), Image.Resampling.LANCZOS)
        expected = gif_frame(rgba, args.alpha_threshold).convert("RGBA")
        current_bytes = expected.tobytes()

        if current_bytes == previous_bytes:
            expected_durations[-1] += durations[index]
        else:
            expected_frames.append(expected)
            expected_durations.append(durations[index])
            previous_bytes = current_bytes

    mismatched_frames = 0
    alpha_mismatch_pixels = 0
    transparent_counts: list[int] = []
    corners_transparent: list[bool] = []
    corners_match_source: list[bool] = []
    decoded_durations: list[int] = []

    with Image.open(args.gif) as encoded:
        encoded_frame_count = encoded.n_frames
        gif_size = encoded.size
        loop = encoded.info.get("loop")
        transparent_index = encoded.info.get("transparency")

        for index in range(encoded_frame_count):
            encoded.seek(index)
            actual = encoded.convert("RGBA")
            decoded_durations.append(int(encoded.info.get("duration", 0)))
            alpha = actual.getchannel("A")
            transparent_counts.append(alpha.histogram()[0])
            corners_transparent.append(
                all(
                    actual.getpixel(point)[3] == 0
                    for point in ((0, 0), (width - 1, 0), (0, height - 1), (width - 1, height - 1))
                )
            )

            if index >= len(expected_frames):
                mismatched_frames += 1
                continue

            expected = expected_frames[index]
            corner_points = ((0, 0), (width - 1, 0), (0, height - 1), (width - 1, height - 1))
            corners_match_source.append(
                all(actual.getpixel(point)[3] == expected.getpixel(point)[3] for point in corner_points)
            )
            if ImageChops.difference(expected, actual).getbbox() is not None:
                mismatched_frames += 1

            expected_alpha = expected.getchannel("A")
            actual_alpha = actual.getchannel("A")
            alpha_mismatch_pixels += sum(
                1
                for expected_value, actual_value in zip(
                    expected_alpha.get_flattened_data(),
                    actual_alpha.get_flattened_data(),
                )
                if expected_value != actual_value
            )

    report = {
        "passed": False,
        "format": "GIF",
        "size": list(gif_size),
        "source_frames": source_frame_count,
        "expected_encoded_frames": len(expected_frames),
        "encoded_frames": encoded_frame_count,
        "source_duration_ms": sum(durations),
        "encoded_duration_ms": sum(decoded_durations),
        "loop": loop,
        "transparent_index": transparent_index,
        "all_frames_have_transparent_pixels": all(count > 0 for count in transparent_counts),
        "minimum_transparent_pixels_per_frame": min(transparent_counts),
        "all_frame_corners_transparent": all(corners_transparent),
        "all_frame_corners_match_source": all(corners_match_source),
        "mismatched_rgba_frames": mismatched_frames,
        "alpha_mask_mismatch_pixels": alpha_mismatch_pixels,
        "frame_durations_match": decoded_durations == expected_durations,
    }
    report["passed"] = all(
        (
            gif_size == (width, height),
            encoded_frame_count == len(expected_frames),
            sum(decoded_durations) == sum(durations),
            transparent_index == 255,
            report["all_frames_have_transparent_pixels"],
            report["all_frame_corners_match_source"],
            mismatched_frames == 0,
            alpha_mismatch_pixels == 0,
            report["frame_durations_match"],
        )
    )

    args.report.write_text(json.dumps(report, indent=2), encoding="utf-8")
    print(json.dumps(report, indent=2))
    if not report["passed"]:
        raise SystemExit(1)


if __name__ == "__main__":
    main()
