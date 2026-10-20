# MAC-OS gcal

Create a Google Calendar event from the macOS terminal in one line — no API keys, no OAuth.

It opens a pre-filled "new event" page in your default browser; you just press **Save**.

```bash
gcal 1530 "call Bob"               # today 15:30 (rolls to tomorrow if already passed)
gcal 330pm "review"                # 15:30
gcal 30m "stretch"                 # 30 minutes from now (also 1h, 1h30m, 90s)
gcal tmr 9am "dentist"             # tomorrow 09:00
gcal 2026-10-25 14:00 "PMLE exam"  # specific date
gcal -d 45 1530 "1:1 w/ lead"      # -d = duration in minutes (default 15)
gcal -a 2026-10-25 "Holiday"       # all-day event
```

## Time formats

| Input | Meaning |
|---|---|
| `1530`, `930` | 15:30, 09:30 |
| `15:30` | 15:30 |
| `3pm`, `330pm`, `3:30pm`, `12am` | 15:00, 15:30, 15:30, 00:00 |
| `30m`, `1h`, `1h30m`, `90s` | relative to now |

Date prefix (optional): `tmr` / `tomorrow` / `YYYY-MM-DD`.
Without a date, a clock time that has already passed today is moved to tomorrow.

## Install

```bash
git clone https://github.com/<you>/gcal.git
cd gcal && ./install.sh          # symlinks gcal into ~/bin
```

Make sure `~/bin` is on your `PATH` (add `export PATH="$HOME/bin:$PATH"` to `~/.zshrc` or `~/.bash_profile`).

## Reminders

The Google Calendar URL API cannot set a per-event notification, so events use your calendar's
**default notification** (Calendar → Settings → Event settings → Notifications). Set it once, e.g. 10 minutes.

## Requirements

- macOS (uses BSD `date` and `open`)
- `python3` (for URL-encoding the title; ships with Xcode CLT)

## License

MIT
