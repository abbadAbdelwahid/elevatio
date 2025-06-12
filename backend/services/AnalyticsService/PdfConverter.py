# Scripts/generate_pdf.py

import PyPDF2
import sys 

# Fonction pour convertir HTML en PDF
def html_to_pdf(html_content, output_path):
    PyPDF2.from_string(html_content, output_path)

if __name__ == '__main__':
    html_content = sys.argv[1]  # Contenu HTML passé en argument
    output_path = sys.argv[2]  # Chemin de sortie du fichier PDF
    html_to_pdf(html_content, output_path) 
    print("Successfully converted html to pdf") 
