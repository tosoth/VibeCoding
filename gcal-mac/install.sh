#!/bin/sh
# Symlink gcal into ~/bin
set -e
cd "$(dirname "$0")"
mkdir -p "$HOME/bin"
ln -sfn "$(pwd)/gcal" "$HOME/bin/gcal"
chmod +x gcal
echo "linked: $HOME/bin/gcal -> $(pwd)/gcal"
case ":$PATH:" in
  *":$HOME/bin:"*) ;;
  *) echo 'note: add  export PATH="$HOME/bin:$PATH"  to your ~/.zshrc or ~/.bash_profile' ;;
esac
