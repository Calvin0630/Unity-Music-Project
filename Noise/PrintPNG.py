# Extracting pixels from an image ------
from PIL import Image
fileName = "noise-perlin2"
image = Image.open(fileName + ".png")
pixels = list(image.getdata())
width, height = image.size
pixels = [pixels[i * width:(i + 1) * width] for i in range(height)]


text_file = open(fileName+".txt", "w")



for i in range(width) :
	line = ""
	for j in range(height) :
		text_file.write(str(pixels[i][j]) + "\n")
	

	
text_file.close()