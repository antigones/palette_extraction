# Palette Extraction (C#)

A simple implementation of Histogram and Median-Cut algorithms to extract a palette from an image. 🌈

 - Histogram algorithm just splits the R,G,B space into n bins and assigns every color to a bin. All the colors in a bin are then averaged to produce a final color for the palette.
 - Median cut algorithm recursively splits the R,G,B space in bins of different sizes. All the colors in a bin are then averaged to produce a final color for the palette.

Reference:
https://spin.atomicobject.com/2016/12/07/pixels-and-palettes-extracting-color-palettes-from-images/
