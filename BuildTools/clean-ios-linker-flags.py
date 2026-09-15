#!/usr/bin/env python3
import pathlib
import re
import sys


def find_named_block(text, start_index):
    depth = 0
    block_start = text.find("{", start_index)
    if block_start == -1:
        return None

    for index in range(block_start, len(text)):
        char = text[index]
        if char == "{":
            depth += 1
        elif char == "}":
            depth -= 1
            if depth == 0:
                return block_start, index + 1

    return None


def find_unity_framework_target(text):
    match = re.search(
        r"([A-F0-9]{24}) /\* UnityFramework \*/ = \{[^}]*?isa = PBXNativeTarget;[^}]*?name = UnityFramework;",
        text,
        re.DOTALL,
    )
    if not match:
        return None

    target_block = find_named_block(text, match.start())
    if not target_block:
        return None

    block = text[target_block[0] : target_block[1]]
    config_match = re.search(r"buildConfigurationList = ([A-F0-9]{24}) /\*", block)
    if not config_match:
        return None

    return config_match.group(1)


def get_configuration_ids(text, config_list_id):
    config_list_match = re.search(
        rf"{re.escape(config_list_id)} /\* Build configuration list.*? = ",
        text,
    )
    if not config_list_match:
        return []

    block_bounds = find_named_block(text, config_list_match.start())
    if not block_bounds:
        return []

    block = text[block_bounds[0] : block_bounds[1]]
    return re.findall(r"([A-F0-9]{24}) /\* (?:Debug|Release|ReleaseForProfiling|ReleaseForRunning) \*/", block)


def should_remove_flag(flag):
    cleaned = flag.strip().strip('"')
    return "-lSystem" in cleaned or cleaned.startswith("-ld") or cleaned.endswith("-ISystem")


def clean_other_ldflags_in_block(block):
    pattern = re.compile(r"OTHER_LDFLAGS = \((.*?)\);", re.DOTALL)

    def replace_flags(match):
        lines = match.group(1).splitlines()
        kept_lines = []
        removed = []

        for line in lines:
            token = line.strip().rstrip(",")
            if token and should_remove_flag(token):
                removed.append(token)
                continue
            kept_lines.append(line)

        if not removed:
            return match.group(0)

        for token in removed:
            print(f"Removed UnityFramework OTHER_LDFLAGS entry: {token}")

        return "OTHER_LDFLAGS = (" + "\n".join(kept_lines) + "\n\t\t\t\t);"

    return pattern.sub(replace_flags, block)


def clean_project(project_path):
    text = project_path.read_text(encoding="utf-8")
    config_list_id = find_unity_framework_target(text)
    if not config_list_id:
        print("UnityFramework target was not found. No linker flags changed.")
        return False

    config_ids = get_configuration_ids(text, config_list_id)
    if not config_ids:
        print("UnityFramework build configurations were not found. No linker flags changed.")
        return False

    changed = False
    for config_id in config_ids:
        config_match = re.search(rf"{re.escape(config_id)} /\* .*? \*/ = ", text)
        if not config_match:
            continue

        block_bounds = find_named_block(text, config_match.start())
        if not block_bounds:
            continue

        original_block = text[block_bounds[0] : block_bounds[1]]
        cleaned_block = clean_other_ldflags_in_block(original_block)
        if cleaned_block != original_block:
            text = text[: block_bounds[0]] + cleaned_block + text[block_bounds[1] :]
            changed = True

    if changed:
        project_path.write_text(text, encoding="utf-8")
        print("Removed bad UnityFramework linker flags.")
    else:
        print("No bad UnityFramework linker flags found.")

    return changed


def main():
    if len(sys.argv) != 2:
        print("Usage: clean-ios-linker-flags.py /path/to/Unity-iPhone.xcodeproj/project.pbxproj")
        return 2

    project_file = pathlib.Path(sys.argv[1])
    if not project_file.exists():
        print(f"Xcode project file not found: {project_file}")
        return 1

    clean_project(project_file)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
