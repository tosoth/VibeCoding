# Apple Reader

A single-file, zero-dependency Markdown reader that mimics the reading experience of Apple Books on iPad. Paste or drop Markdown, and read it in a clean, distraction-free layout.

No build step, no server, no external libraries — just open `apple_reader.html` in a browser. Works fully offline.

## Features

### Markdown rendering
A built-in, dependency-free Markdown renderer supports:

- Headings (ATX `#` and Setext underline styles)
- **Bold**, *italic*, ~~strikethrough~~, `inline code`
- Fenced code blocks
- Links, images, and auto-linked URLs
- Blockquotes
- Ordered, unordered, nested, and task lists (`- [x]`)
- Tables with column alignment
- Horizontal rules

### Apple Books–style typography
- Reading column width of 820px with 56px side margins (narrows automatically on small screens)
- 21px base font size with 1.6 line height
- Serif by default (New York / Iowan Old Style / Palatino / Georgia), switchable to sans-serif (San Francisco / system UI)
- Four themes matching Apple Books: **White**, **Sepia**, **Gray**, and **Night**
- Adjustable font size (15–34px)
- Frosted-glass toolbar that appears on mouse movement and hides after ~2 seconds of inactivity

### Editing & input
- **Paste** Markdown directly into the page (`⌘V` / `Ctrl+V`) — it renders immediately
- **Drag and drop** a `.md` or `.txt` file onto the window
- Toggle a raw Markdown editor to make changes, then return to the reading view
- Content, theme, font, and font size are persisted in `localStorage` and survive reloads

### Fullscreen
Press `Tab` to toggle fullscreen reading mode.

## Keyboard shortcuts

| Key | Action |
|---|---|
| `Tab` | Toggle fullscreen (in edit mode: insert indent; use `Shift+Tab` for fullscreen) |
| `⌘E` / `Ctrl+E` | Toggle Markdown editor |
| `Esc` | Exit editor and return to reading view |
| `⌘V` / `Ctrl+V` | Paste and render Markdown (reading mode) |
| `+` / `=` | Increase font size |
| `-` | Decrease font size |
| `T` | Cycle theme (White → Sepia → Gray → Night) |
| `F` | Toggle serif / sans-serif |

## Usage

1. Download or clone this repository.
2. Open `apple_reader.html` in any modern browser (Safari, Chrome, Firefox, Edge).
3. Paste Markdown text or drop a `.md` file onto the page.
4. Press `Tab` to go fullscreen and start reading.

```bash
git clone https://github.com/<your-username>/apple_reader.git
open apple_reader/apple_reader.html
```

## Project structure

```
apple_reader/
├── apple_reader.html   # The entire app: HTML, CSS, Markdown renderer, and UI logic
└── README.md
```

## Browser support

Any modern browser with ES6 and `backdrop-filter` support. The typography looks best on macOS / iOS, where the New York and San Francisco fonts are available; other platforms fall back to Georgia / system sans-serif.

## License

MIT
