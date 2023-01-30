# EF
## DBの型とEntityの型の違いを吸収する仕組み
ValueConverterを使用する  
[値の変換](https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations)  
[EntityFrameworkCoreのValueConverterを試す](https://noxi515.hateblo.jp/entry/2019/02/10/164936#%E7%B5%84%E3%81%BF%E8%BE%BC%E3%81%BF%E3%81%AEValueConverter%E3%82%92%E5%88%A9%E7%94%A8%E3%81%99%E3%82%8B)  

DBには0/1で登録しているが、Entityはフラグ(bool)として使われているみたいな場合や、  
DBはintで登録しているが、EntityはEnumとして使われているみたいな場合に、  
Entity側で変換するのではなく、Fluent APIでEntityに値を詰める際に自動で変換してくれる。  

## EFを使った値の取得
DbContexの派生クラスに定義したDbSetプロパティに対して、以下の操作をした場合にクエリが実行される。
- foreach (C#) または For Each (Visual Basic) ステートメントによって列挙された場合。
- ToArray、ToDictionary、ToList などのコレクション操作によって列挙された場合。
- First や Any などの LINQ 演算子がクエリの最も外側で指定された場合。

コンテキストの有効期間は、インスタンスが作成された時点から、インスタンスが破棄またはガベージ コレクトされた時点まで。
よって基本的にはContextはUsingで生成することが望まれる。

```title:C#
public void UseProducts()
{
    using (var context = new ProductContext())
    {     
        // Perform data access using the context
    }
}
```

[DbContext の操作](https://learn.microsoft.com/ja-jp/ef/ef6/fundamentals/working-with-dbcontext)  

## FluentAPIでのリレーションシップの設定方法
以下を使用して設定する。
- HasOne/HasMany
- WithOne
- HasForeignKey

[Entity Framework Coreでリレーションシップを設定する方法](https://tech-blog.cloud-config.jp/2022-11-07-entity-framework-core-relationship/)