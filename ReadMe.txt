THIS SOFTWARE IS DISTRIBUTED AS IS.
NO WARRANTY IS GIVEN ABOUT FUNCTIONALITY AND SAFETY OF THIS SOFTWARE.

This SM64 Hacking Toolkit contains a model importer, a level script editor as well as a small patching engine for custom changes.
The model importer can currently import .obj files as display lists and collision maps.
The level script editor is in its early stages and can really just manipulate object placement at this point.
The patch engine doesn't do much other than apply byte changes that you can also read out manually and apply with a hex editor.

Stuff that will have to be done:
	-ModelImporter
		Implement all Texture formats for the model importer.
		Implement Water Boxes and other special collision.
		Create some sort of help file to explain functionality (lol)
		
	-LevelEditor
		Implement Area Editing (this is probably a 2 Minute thing lol)
		Implement Background Changing
		Implement ability to change loaded RAM Banks
		Implement Music ability to change Music
		
	-Other
		Document things.