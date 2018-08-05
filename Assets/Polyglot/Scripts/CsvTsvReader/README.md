# UnityCSVReader
CSV Reader for Unity(implemented using C#)

If you want stable module, use https://github.com/JoshClose/CsvHelper

This CSVReader interprets quoted-string, escaped double quote and new line.  This class is implemented in a single file.  You can put (and remove) this class easily.

## How to use

```
// using System.IO;
var result = CSVReader.ParseCSV(File.ReadAllText(@"/Users/furukazu/data.csv"));

foreach(var line in result){
    foreach(var col in line){
        // process the data
    }
}

// read from Resource
// in this case, you place the csv file at Assets/Resources/CSV/data.csv 
var res = CSVReader.ParseCSV((Resources.Load("CSV/data") as TextAsset).text);

```

## License
MIT



# UnityCSVReader
Unity用のCSV読み込み機構の実装(C#で作りました)

ちゃんとしたのが欲しかったら→ https://github.com/JoshClose/CsvHelper これ使った方が良いと思います。

この CSVReader は " や改行が含まれている CSV ファイルを解釈できます。また、単一のファイルによる実装なので、導入や除去が簡単です。雑にコピペして入れることもできます。

## 使い方

```
// using System.IO;
var result = CSVReader.ParseCSV(File.ReadAllText(@"/Users/furukazu/data.csv"));

foreach(var line in result){
    foreach(var col in line){
        // 内容を処理する
    }
}

// Resource から読む場合
// Assets/Resources/CSV/data.csv があるものとします
var res = CSVReader.ParseCSV((Resources.Load("CSV/data") as TextAsset).text);

```

## License
MIT

