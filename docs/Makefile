# Install required Python dependencies (MkDocs etc.)
prepare-unix:
	python3 -m venv venv;
	source venv/bin/activate;
	pip3 install -r requirements.txt;

prepare-windows:
	python -m venv venv;
	.\venv\Scripts\activate;
	pip install -r requirements.txt;

# Run the mkdocs development server
serve:
	mkdocs serve
