#!/usr/bin/env python3
"""Convert a row-major Lottie RGBA atlas into an alpha-safe animated GIF."""

from __future__ import annotations

import argparse
import json
import math
from pathlib import Path

from PIL import Image


TRANSPARENT_INDEX = 255


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser()
    parser.add_argument("--lottie", required=True, type=Path)
    parser.add_argument("--sheet", required=True, type=Path)
    parser.add_argument("--output", required=True, type=Path)
    parser.add_argument("--columns", type=int, default=11)
    parser.add_argument("--rows", type=int, default=11)
    parser.add_argument("--alpha-threshold", type=int, default=127)
    return parser.parse_args()


def gif_frame(rgba: Image.Image, alpha_threshold: int) -> Image.Image:
    """Quantize to 255 colors and reserve palette index 255 for transparency."""
    alpha = rgba.getchannel("A")
    opaque_mask = alpha.point(
        lambda value: 255 if value > alpha_threshold else 0,
        mode="1",
    )

    quantized = rgba.convert("RGB").quantize(
        colors=255,
        method=Image.Quantize.MEDIANCUT,
        dither=Image.Dither.FLOYDSTEINBERG,
    )
    palette = quantized.getpalette()[: 255 * 3] + [0, 0, 0]

    frame = Image.new("P", rgba.size, TRANSPARENT_INDEX)
    frame.putpalette(palette)
    frame.paste(quantized, mask=opaque_mask)
    frame.info["transparency"] = TRANSPARENT_INDEX
    frame.info["disposal"] = 2
    return frame


def frame_durations(frame_count: int, fps: float) -> list[int]:
    """Distribute GIF centiseconds without accumulating timing drift."""
    return [
        (round((index + 1) * 100 / fps) - round(index * 100 / fps)) * 10
        for index in range(frame_count)
    ]


def main() -> None:
    args = parse_args()
    if not 0 <= args.alpha_threshold <= 254:
        raise ValueError("--alpha-threshold must be between 0 and 254")

    metadata = json.loads(args.lottie.read_text(encoding="utf-8"))
    width = int(metadata["w"])
    height = int(metadata["h"])
    fps = float(metadata["fr"])
    frame_count = int(math.ceil(float(metadata["op"]) - float(metadata["ip"])))

    sheet = Image.open(args.sheet).convert("RGBA")
    if sheet.width % args.columns or sheet.height % args.rows:
        raise ValueError("Sprite sheet size is not divisible by the atlas grid")
    if frame_count > args.columns * args.rows:
        raise ValueError("Atlas grid does not contain every Lottie frame")

    cell_width = sheet.width // args.columns
    cell_height = sheet.height // args.rows
    frames: list[Image.Image] = []

    for index in range(frame_count):
        column = index % args.columns
        row = index // args.columns
        bounds = (
            column * cell_width,
            row * cell_height,
            (column + 1) * cell_width,
            (row + 1) * cell_height,
        )
        rgba = sheet.crop(bounds)
        if rgba.size != (width, height):
            rgba = rgba.resize((width, height), Image.Resampling.LANCZOS)
        frames.append(gif_frame(rgba, args.alpha_threshold))

    args.output.parent.mkdir(parents=True, exist_ok=True)
    durations = frame_durations(frame_count, fps)
    frames[0].save(
        args.output,
        save_all=True,
        append_images=frames[1:],
        duration=durations,
        loop=0,
        disposal=2,
        transparency=TRANSPARENT_INDEX,
        optimize=False,
    )

    with Image.open(args.output) as encoded:
        encoded_frame_count = encoded.n_frames

    print(
        json.dumps(
            {
                "output": str(args.output.resolve()),
                "size": [width, height],
                "source_frames": frame_count,
                "encoded_frames": encoded_frame_count,
                "fps": fps,
                "duration_ms": sum(durations),
                "transparent_index": TRANSPARENT_INDEX,
            },
            indent=2,
        )
    )


if __name__ == "__main__":
    main()
