#!/usr/bin/env bash
# Copy new image sequences from ../art
cp -ruv ../art/sprites/. Content/Sprites/
# Run spriteAtlasPacker
Content/SpriteAtlasPacker.exe -image:Content/animations.png -map:Content/animations.atlas -fps:24 Content/Sprites
echo "You should rebuild animations.atlas and animations.png in the Pipeline Tool"
