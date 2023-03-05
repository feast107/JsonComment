# JsonComment
能够通过注解自动生成文档注释的基于 `.NET Standard 2.0` 的代码分析器

:construction: :construction: **施工中注意——仍有bug** :construction: :construction:

# Effect：
![](/doc/Example.png)

# Start
+ 引用程序集之后添加为 
    ```json
    OutputItemType="Analyzer"
    ```
    ``` xml
    <ItemGroup>
        <ProjectReference Include="path\to\Feast.JsonComment.csproj" OutputItemType="Analyzer" />
    </ItemGroup>
    ```
    由于需要引用包中的内容，请忽略 `ReferenceOutputAssembly` 项或将其置为 `true` 
    ``` json
    ReferenceOutputAssembly="true"
    ```
+ 在你需要生成注释的类型上添加 `JsonComment`
    ``` c#
    using Feast.JsonComment;

    [JsonComment]
    public partial class MyModel{
        public int Id { get; init; } 
        public string Name { get; set; } 
    } 
    ```
    + :warning: **类型需要携带 `partial` 关键字**
    + :warning: **类型的其他注释将会覆盖该注释**
    + :ok_hand: 本程序集不会对任何类型进行增改


+ 在你的程序中调用 `JsonComment.Generate()`
    ``` c#
    internal class Program
    {
        static void Main(string[] args)
        {
            JsonComment.Generate();
        }
    }
    ```
    文档将会生成在类型声明文件所在的目录下，目录下批量的源文件会整合到一个文档文件中


# Preview
+ :grey_exclamation: 泛型<>
+ :construction: 初始值项的设定
+ :construction: 如果尽可能的避免生成外部文档，但这会要求在分析时生成JSON数据，以及继承关系分析上的困难