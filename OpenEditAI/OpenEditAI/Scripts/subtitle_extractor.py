import os
import re
import sys
import argparse
from openai import OpenAI
from config import config

def print_error(message):
    print(message, file=sys.stderr)

def subtitle_extractor(input_file, prompt):
    if not os.path.isfile(input_file):
        print_error(f"The input path {input_file} is not a file.")
        return None

    client = OpenAI(api_key=config.OPENAI_API_KEY)

    response = ""
    with open(input_file, 'r') as file:
        # Read the entire contents of the file
        response = client.chat.completions.create(
            model="gpt-3.5-turbo",
            messages=[
                {"role": "system", "content": prompt},
                {"role": "user", "content": file.read()}
            ]
        )
    return re.findall(r'\d+(?=,|$)', response.choices[0].message.content)

def main():
    parser = argparse.ArgumentParser(description='Extract subtitles from an audio file.')
    parser.add_argument('input_file', type=str, help='The path to the extracted subtitles from.')
    parser.add_argument('prompt', type=str, help='Prompt is missing. Please provide a prompt.')

    args = parser.parse_args()
    output_file = subtitle_extractor(args.input_file, args.prompt)

    sys.stdout.write(str(output_file))

if __name__ == "__main__":
    main()
