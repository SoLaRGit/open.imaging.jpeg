
PROJECT INTENT:
===============

Original intent of this project is not to decode JPEG files, but rather
to decode parts of JPEG file rarely decoded by other JPEG libraries.

As project's primary focus was on Metadata information within segments 
contained inside JPEG file itself. As stated above many libraries pay no 
attention to this segments, or give you control over them. 
I wanted to make library capable of preserving original segments and resample 
image content. And or preserve original image content and strip metadata 
information. Or extract relevant information about image, for example GPS 
location where image was taken and similar.
And to fully dive into standards surrounding JPEG file format in all.

Also intent was focused on making this library work as fast as possible.
For example extracting only relevant data, by skipping read of unnecessary 
segments, and reading only relevant data end user/developer wants.

Since library goal is not JPEG Image Data decoding/encoding, and If you 
wish to decode JPEG image data use some other library, for example I've 
been using for ages old high performance IJL Intel JPEG Library (1.5) 
but noticed there are more and more images with errorness encodings not 
supported by IJL. Switched to libJPEG-turbo library that has similar 
performance but correctly decodes errorness encodings. That has a lot of 
promises for the future of that library. The team is trying to write all 
of functions in libJPEG-turbo library to use CPU SIMD extensions to 
improve on performance when working with JPEG files.
There are many other JPEG libraries also to be taken into consideration.

Random Notices over years:
--------------------------

-

At the beginning of writing this project there was only one more similar 
project trying to do the same. As it was written in C++, and not portable 
to .Net platform, project open.imaging.jpeg was born.

And almost everything you see in this project here was done in 2010, and
few more options and very few bug fixes (actually support for loading
errorness encodings in jpeg files) in following year. After this there 
was almost no rewrites of code, but only pure usage on daily basis. 
Think this code actually have read, decoded, extracted, repack, and 
written several million jpeg images, if not even more. 
Unfortunately there is no real statistics on usage of this code.

But please notice all other projects that were consuming this library are x86 
based, since they are backdating 2 decades while x64 was only imaginary thing
at the time. 
So to be aware don't think anyone has properly tested for x64 platform.

-

Encountered errorness encoded JPEG images. Tweaking reader to consume these
files while only reporting back to user/developer error in encoding.

-

Library implemented in several other projects:

Image Metadata editor uses this library for editing and bulk dumping image 
file metadata. Only direct result of this library, as been created after this
library.

Image similarity front end uses this library as COM component when comparing 
internal data structures for Image Content similarity skipping differences in
metadata information as Image Content difference.
Fixing problem with False Positive differences when actual image data was 
exactly the same. Meaning there is some software in wilderness that actually
does no decoding/encoding of jpeg image data. But only repacks, adds, or 
strips other jpeg file segments. Having similar capability as this library.

Image resample front end uses this library as COM component for repacking 
and preserving original image file metadata, while using more advanced libs
for encoding and decoding of jpeg image data.

-

There is also plan to release it to open source society for free usage.
Guess someday after proper implementation of standards and testing...

Do the library code infully first...











