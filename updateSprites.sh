#!/usr/bin/env bash
# Convert gif to png sequence using ffmpeg
# ffmpeg -i ../art/assets/images/rain-transparent.gif  -vsync 0 ../art/sprites/rain/rain.00%d.png
# Copy new image sequences from ../art
cp -ruv ../art/sprites/. Content/Sprites/
# Run spriteAtlasPacker
echo "Running SpriteAtlasPacker"
Content/SpriteAtlasPacker.exe -image:Content/animations.png -map:Content/animations.atlas -fps:24 Content/Sprites
echo "You should rebuild animations.atlas and animations.png in the Pipeline Tool"
