# VideoCutter / 视频截取工具

A tiny Windows GUI for cutting a segment out of a video with ffmpeg. By default it copies streams without re-encoding, so quality is untouched and file size does not grow.

一个小巧的 Windows 图形界面工具，用 ffmpeg 从视频中截取一段。默认直接复制音视频流、不重新编码，画质无损、体积不增加。

---

## English

### Features

- Pick any file ffmpeg supports (mp4, mkv, mov, avi, webm, ts, ...) with a file dialog
- Duration is read automatically via `ffprobe`
- One range slider with two thumbs to choose start and end
- Start / end times can also be typed directly (click the time box, press Enter)
- Output file name is auto-derived as `<source>_cut1.mp4`, `_cut2`, ... (first unused name) and can be edited
- Runs ffmpeg in the background, so the window never freezes
- Optional frame-accurate mode (re-encodes)

### Requirements

- Windows with .NET Framework 4.x (built into Windows 10/11)
- `ffmpeg.exe` and `ffprobe.exe`, either in the same folder as `VideoCutter.exe` or on your `PATH`

### Build

1. Put `VideoCutter.cs` and `build.bat` in the same folder.
2. Double-click `build.bat`. It uses the C# compiler that ships with Windows (`csc.exe`) and produces `VideoCutter.exe`. No Visual Studio needed.

### Usage

1. Click **选择文件... (Select file)** and choose a video.
2. Drag the two thumbs, or type times into the start / end boxes.
   Accepted formats: `1:08:58`, `05:30.5`, `90` (h:m:s, m:s.fraction, plain seconds). Enter confirms, Esc cancels.
3. Check or edit the output path.
4. Click **生成 (Generate)**. When it finishes, the output name advances to the next `_cutN`, ready for the next segment.

### Notes

- **Copy mode (default):** cut points snap to nearby keyframes, so start and end can be off by a fraction of a second up to a few seconds, and the first moments may glitch. This is a limit of cutting without re-encoding.
- **Frame-accurate mode:** tick "精确到帧" to re-encode. It is slower and the file size and quality will change.
- The output file must differ from the source. If it already exists you will be asked before overwriting.
- Command used (copy mode):
  `ffmpeg -y -ss <start> -i <input> -t <length> -c copy -avoid_negative_ts make_zero <output>`

### Troubleshooting

- *"找不到 ffprobe" / ffprobe not found:* put `ffmpeg.exe` and `ffprobe.exe` next to the program or add them to `PATH`.
- *Output won't play for some formats:* copying certain codecs into `.mp4` can fail (e.g. from some mkv files). Change the output extension to match the source, or use frame-accurate mode.

---

## 中文

### 功能

- 通过文件对话框选择 ffmpeg 支持的任意文件（mp4、mkv、mov、avi、webm、ts 等）
- 选中后自动用 `ffprobe` 读取时长
- 一条滑动条上有两个滑块，分别选择开始和结束时间
- 开始 / 结束时间也可以直接输入（点击时间框，按回车确认）
- 输出文件名自动生成为 `源文件_cut1.mp4`、`_cut2`……（取第一个未被占用的名字），可自行修改
- ffmpeg 在后台运行，界面不会卡住
- 可选"精确到帧"模式（会重新编码）

### 运行要求

- Windows，且带有 .NET Framework 4.x（Windows 10/11 自带）
- `ffmpeg.exe` 和 `ffprobe.exe`，放在 `VideoCutter.exe` 同一目录，或加入系统 `PATH`

### 编译

1. 把 `VideoCutter.cs` 和 `build.bat` 放在同一个文件夹。
2. 双击 `build.bat`，它会调用 Windows 自带的 C# 编译器（`csc.exe`）生成 `VideoCutter.exe`，无需安装 Visual Studio。

### 使用方法

1. 点击 **选择文件...**，选择视频。
2. 拖动两个滑块，或在开始 / 结束输入框里输入时间。
   支持的格式：`1:08:58`、`05:30.5`、`90`（时:分:秒、分:秒.小数、纯秒数）。回车确认，Esc 取消。
3. 检查或修改输出路径。
4. 点击 **生成**。完成后输出文件名会自动顺延到下一个 `_cutN`，方便连续截取多段。

### 注意事项

- **复制模式（默认）：** 切点会对齐到附近的关键帧，所以起止位置可能有零点几秒到几秒的偏差，开头也可能出现短暂花屏。这是不重新编码时的固有限制。
- **精确到帧模式：** 勾选"精确到帧"会重新编码，速度较慢，体积和画质也会变化。
- 输出文件不能与源文件相同；如果输出文件已存在，覆盖前会先询问。
- 复制模式实际执行的命令：
  `ffmpeg -y -ss <开始> -i <输入> -t <时长> -c copy -avoid_negative_ts make_zero <输出>`

### 常见问题

- *提示"找不到 ffprobe"：* 把 `ffmpeg.exe` 和 `ffprobe.exe` 放到程序同目录，或加入 `PATH`。
- *某些格式输出后无法播放：* 把某些编码直接复制进 `.mp4` 可能失败（例如部分 mkv）。可以把输出扩展名改成与源文件一致，或使用"精确到帧"模式。
