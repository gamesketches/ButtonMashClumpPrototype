import os, json, sys

def twine2CSV(filename, outputFilename):
    input = open(filename, "r")
    fullText = input.readlines()
    fullText = fullText[364]
    input.close()
    allLines = fullText.split('<')[2:]
    data = {}
    for curLine in allLines:
        if curLine == '/div>':
            continue
        firstQuotation = curLine.find('\"') + 1
        if firstQuotation == 0:
            continue
        key = curLine[firstQuotation : firstQuotation + curLine[firstQuotation:].index('\"') ]
        if key == "StoryAuthor":
            continue
        sentence = curLine[curLine.index('>') + 1 : ]
        scriptLines = sentence.split('.')
        fullySplitLines = []
        for line in scriptLines:
            fullySplitLines += line.split('\\n')

        fullySplitLines = [x for x in fullySplitLines if x != ""]
        horusKey = "gameOver"
        setKey = "gameOver"
        print fullySplitLines
        if fullySplitLines[-2].find("|") > 0:
            pipeIndex = fullySplitLines[-2].find("|") + 1
            horusKey = fullySplitLines[-2][pipeIndex : fullySplitLines[-2].find("]")]
            print horusKey
            pipeIndex = fullySplitLines[-1].find("|") + 1
            setKey = fullySplitLines[-1][pipeIndex : fullySplitLines[-1].find("]")]
            print setKey
            fullySplitLines = fullySplitLines[:-2]

        data[key] = {"lines":fullySplitLines, "horusKey":horusKey, "setKey":setKey}

    data_string = json.dumps([data], indent=2)
    outputFile = open(outputFilename + ".json", "w")
    outputFile.write(data_string)


if __name__ == "__main__":
   twine2CSV(sys.argv[1], sys.argv[2])
