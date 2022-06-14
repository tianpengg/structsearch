# 结构式搜索

基于 .NET CORE 3.1 实现结构式搜索的DEMO，开箱即用。
- 实现结构式入库、更新、删除
- 实现结构式精确、子结构、相似度搜索
- 跨平台部署运行
- docker的支持

## 引用相关技术方案

- [indigo](https://lifescience.opensource.epam.com/index.html)
- [rdkit](http://www.rdkit.org)
- [rdkit-build-C#](https://github.com/kazuyaujihara/build-rdkit)
## NuGet

````
<ItemGroup>
		<PackageReference Include="Indigo.Net" Version="1.6.1" />               
		<PackageReference Include="RDKit.DotNetWrap" Version="0.2021094.2" />
</ItemGroup>
````

## 提示

1. 请结合自己需求进行修改，如分库的逻辑
2. 关于反应式搜索，如果传递的参数本来就是一个错误的Smiles，底层indigo是直接报错

## 感谢
1. 由衷感谢`kazuyaujihara`大佬编译的rdkit，可放心大胆的跨平台使用
2. 感谢indigo团队对化学方向的研究

## 规划
1. 针对indigo-ES的方案修正，将本地文件数据库替换到elasticsearch中
2. 添加结构式转图片、结构式分子量的计算等API接口

欢迎有兴趣的小伙伴一起研究
