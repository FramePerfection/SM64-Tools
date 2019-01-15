THIS SOFTWARE IS DISTRIBUTED AS IS.
NO WARRANTY IS GIVEN ABOUT FUNCTIONALITY AND SAFETY OF THIS SOFTWARE.

This software uses a slightly modified version of the nQuant color quantization software.
For more information, see "License.txt" in the nquant subdirectory.

This SM64 Hacking Toolkit contains a model importer, a level script editor as well as a small patching engine for custom changes.
The model importer can currently import .obj and .dae files as display lists and collision maps.
The level script editor is in its early stages and can really just manipulate object placement at this point.
The patch engine doesn't do much other than apply byte changes that you can also read out manually and apply with a hex editor.

-ModelImporter
	This tool allows importing several model files into the game at once.
	To do this, let the tool run the level script for the specific level you want to import over
	by typing the address of the jump command into the text box at the top, then hit "Run from here."
	
	On the "+" Tab Page you can add objects such as display lists and collision maps, as well as
	fiddly adjust global fog settings which is a joke really.
	For both object types the boxes "Segmented Pointers" and "ROM pointers" determine where to write out
	the pointers to the newly imported objects.
	For example, this may be the address where a 0x15 geo layout command has its pointer.
	Note that this tool does not generate any geo layout scripts (yet?).
	
	You can load a .obj or .dae file by clicking "Load Object".
	For .dae file, you can choose what to do with colored vertices.
	For example, you could convert the vertex color to only vertex alpha while maintaining normals
	for seamless texture transitions.
	This option does nothing for .obj files.
	
	For display lists there's a "Layer" option which should be used to properly reset rendering options
	at the end of the display list in the way the game expects it.
	It achieves this by choosing a particular set of commands to append to the display list, and the value
	you choose in the "Layer"-Box should match with any respective layer values of 0x15/0x13 commands.
	(Have you noticed how in many ROM Hacks, things like coin sparkles or other particles look wrong? This is used to fix that.)

	The "Scrolling Textures" pointer section at the right near the texture preview is per texture.
	However, since there is no good implementation of actual scrolling textures in SM64, neither is in this importer.
	Instead, the importer will write the address of the selected texture's G_SETTILESIZE (0xF2) command.
	(You can write a piece of code that alters the offset values of this command to achieve a scrolling effect.
	For more Information, see https://wiki.cloudmodding.com/oot/F3DZEX/Opcode_Details#0xF2_.E2.80.94_G_SETTILESIZE)
	
	
	Collision Maps work the same as display lists pointer wise, except there is an additional option to use "Special Pointers"
	Special Pointers are pointer addresses as read by the level script reader, and the only currently available option are "Area n"
	where n indicates the index of the area. It is always recommended to use this option if you want to import the collision for
	a level area since the offset of the area collision defining command in the level script may move around during editing.
	
	Notice that you can choose to change collision types by either .obj objects or by material.
	In both cases, you can choose to ignore either a certain object or a certain material using the "Import" checkbox.
	These checkboxes are a bit buggy with saving and loading settings, so pay attention to that.
	
	
	Finally, the bottom options "Base RAM offset" determines in which bank and at which offset to start writing the imported object data.
	Typically for ROM Hacks, this is simply 0x0E000000 (so bank 0xE at offset 0), but you can use any value you want.
	If the bank you're trying to import into is not long enough, the import will fail.
	
	The "Scale" option will determine by which factor to scale vertex data from model files.
	For example, a value of 100 will mean that a cube with width 1 will be 100 units wide after the import.
	There is no overflow protection, so make sure your imported vertices stay between -32768 and 32767!

	
	
Stuff that will have to be done:
	-ModelImporter
		Implement all Texture formats for the model importer.
		Implement Water Boxes and other special collision.
		Create some sort of help file to explain functionality (lol)
		Make it actually useful.
		
	-LevelEditor
		Implement Background Changing (eeeeeeeeeeehhhh???)
		Implement ability to change loaded RAM Banks (Very buggy atm lool)