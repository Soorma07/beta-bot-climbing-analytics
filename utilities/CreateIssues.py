import csv
import subprocess

REPO = "Soorma07/beta-bot-climbing-analytics"

def ensure_label_exists(label):
    result = subprocess.run(
        ["gh", "label", "list", "--repo", REPO],
        capture_output=True, text=True
    )
    existing_labels = [line.split()[0] for line in result.stdout.strip().split("\n") if line]
    if label not in existing_labels:
        subprocess.run([
            "gh", "label", "create", label,
            "--repo", REPO,
            "--color", "ededed",
            "--description", f"{label} label for roadmap"
        ])

with open("issues.csv", newline="", encoding="utf-8") as f:
    reader = csv.DictReader(f)
    for row in reader:
        title = row["Title"].strip()
        body = row["Body"].strip()
        labels = [l.strip() for l in row["Labels"].split(",") if l.strip()]
        milestone = row["Milestone"].strip()

        for label in labels:
            ensure_label_exists(label)

        cmd = [
            "gh", "issue", "create",
            "--repo", REPO,
            "--title", title,
            "--body", body
        ]

        for label in labels:
            cmd += ["--label", label]

        if milestone:
            cmd += ["--milestone", milestone]

        print(f"\nCreating issue: {title}")
        subprocess.run(cmd)
