# BicycleCalculator

[![build status](http://neocosmical.eicp.net:30000/neocosmical/BicycleCalculator/badges/master/build.svg)](http://neocosmical.eicp.net:30000/neocosmical/BicycleCalculator/commits/master)

Bicycle Calculator

自行车计算器

运行环境：
Windows XP/Vista/7/8/8.1 32位/64位

如需安装.NET Framework 4.0可以从微软官网下载：
http://www.microsoft.com/zh-cn/download/details.aspx?id=17718 

使用说明：
1. 选择牙盘、飞轮、内变速、轮胎等型号以及踏频，实时计算出齿比、速比、车速等参数并绘制曲线。
2. 勾选”踏频/车速“中的”车速“，可输入车速，反算所需踏频。
3. 点击”添加当前曲线“记录当前的曲线用于对比，可添加多条曲线。
4. 点击”导出文件“导出当前配置和计算结果，导出的文件需使用Excel打开查看。
5. 对于没有飞轮变速只有内变速的情况，在飞轮处选”1s/内变“，并设置内变速上飞轮的齿数。
6. 右键点击曲线图可选择当前曲线显示模式。
7. 在计算结果表格中选择任意一行，可查看所对应的档位信息和图像上的位置。

Instructions:
1. Select the crankset, Cassette Sprockets, Internal Hub, tires models and cadence, the gear ratio, speed ratio and other parameters will be calculated in real-time and the curve will be drawn automatically.
2. Check "speed" in the group box "cadence / speed", the reverse calculation from speed to cadence will be available.
3. Click "Add current curve" button to add current curve to the chart for comparison, multiple curves can be added.
4. Click "Export File" to export current configuration and calculation results, the exported file can be opened with Excel.
5. For Internal Hub only cases, select "1s /  Internal Hub" in the Cassette Sprockets box and set the number of teeth on cassette.
6. Right-click on the chart to select the display mode of the current curve.
7. Select any row in the results table, the corresponding gears and point on the curve will be highlighted.

更新日志：

2014-12-21
1.5.0
1. 自动保存界面上的部分数据、设置及布局，并在下次打开时自动载入。 目前暂不包括已填加曲线等信息。
2. 启动载入数据时增加loading画面。

2014-12-10
1.4.10
1. 自动保存界面上的部分数据及设置，并在下次打开时自动载入。目前包括以下数据：牙盘、飞轮、内变速、踏频/车速、轮胎。今后将逐渐包括辐条长度、已填加曲线、界面视图等更多数据。
2. 增加花鼓数据。

1.4.9
1. 优化启动速度。

2014-09-22
1.4.8
1. 增加花鼓、轮圈数据来源链接。

2014-09-19
1.4.7
1. 修改个别牙盘数据。
2. 增加大量花鼓、轮圈型号数据。

2014-08-22
1.4.6
1. 添加曲线时可重命名曲线。

2014-07-15
1.4.5
1. 增加若干内变速型号。

2014-07-14
1.4.4
1. 增加若干轮圈及花鼓型号。

2014-07-02
1.4.3
1. 增加辐条张力比计算。

2014-06-23
1.4.2
1. 增加语言选项。
2. 增加若干内变速型号。

2014-06-17
1.4.1
1. 修正手动输入辐条数量时可能出现的bug。

2014-06-16
1.4.0
1. 增加辐条长度计算功能。

2014-06-12
1.3.6
1. 使用WPF设计界面。
2. 修正Windows XP下的不兼容问题。

1.2.3
1. 纵轴增加车速/踏频选项。
2. 修正错误。

2014-06-09
1.2.2
1. 修正齿容计算错误。

2014-06-05
1.2.1
1. 修正牙盘排序。

2014-05-30
1.2.0
1. 增加菜单栏并整合功能按钮。
2. 支持隐藏内变速页面和曲线图。
3. 参数输入控件优化。

2014-05-29
1.1.10
1. Bug修复

2014-05-28
1.1.9
1. 支持km/h、mph单位切换。
2. 导出文件数据涵盖所有表格中的数据。

2014-05-27
1.1.8
1. 表格支持多选，同时计算选中项的变比增量。
2. 增加“档位（齿）”、“增量”数据。
3. 表头包含项目支持自定义
4. 曲线图支持选择区域放大
5. 曲线图和表格支持调整大小
6. 调整运算结果小数位数

1.1.7
增加对英文系统的支持。

2014-05-26
1.1.6
1. 飞轮增加2速和3速选项，为了适应Brompton的外三速和内5外2花鼓。
2. 增加若干轮胎型号。

2014-05-25
1.1.5：
1. 去掉没有实际意义的按飞轮分支显示；
2. 修正某种情况下已添加的曲线不显示的问题。

1.1.4：
1. 添加曲线重名时允许重命名并添加。
2. 添加曲线图右键功能菜单。
3. 曲线纵轴支持选择走距速比、GI速比、齿比。
4. 曲线横轴支持分支显示，按牙盘、飞轮、内变速。
5. 感谢@coolsear  

2014-05-23
1.1.3：
1. 曲线显示单次踩踏行进距离(m)，有物理意义的东西比较好理解。
2. 增加齿轮比的差异总额显示
3. 改善调整参数时运算结果闪烁的问题。
4. 增加捐赠链接

1.1.2：
1. 增加几种内变速型号@hachi  ；
2. 增加几种轮胎型号并标明一些常用品牌@dolphinic；
3. 轮胎使用ISO(ETRTO)方式输入时，胎宽同时支持mm和inch，如47、50或1.1、1.75，自动判断输入的单位并计算周长@bingol 。

2014-05-22
1.1.1：
1. 轮胎选择现在支持使用ISO(ETRTO)方式输入！（例如54-406分别输入54和406）
2. 选中计算结果的任意一行，自动标出相应的牙盘、飞轮和内变速档位，同时在曲线中标出相应点。
3. 优化踏频动态显示效果和精度。
4. 感谢@dolphinic  

1.1.0：
1. 增加内变速档位数选项至内14速。
2. 增加若干内变速型号。
3. 修正内变相关计算公式问题。
4. 增加对内变扭矩过大的警告。
5.  “关于”中增加77bike链接及LOGO。

2014-05-21
1.0.9：
1. 修正导出文件时可能存在的数据错误bug
2. 导出文件中增加轮胎型号及参数
3. 修复某些情况下关于档位不可用判断错误的问题。

1.0.8：
1. 修正内变速档位排列顺序问题。
2. 修正添加曲线名称未包含内变速型号的问题。

1.0.7：
1. 增加若干轮胎数据
2. 现在支持内变速啦！！
3. 增加导出文件功能！！
4. 绿色LOGO
5. 感谢 @babyfish79  

1.0.6：
修正排序时存在的bug

1.0.5：
修复手动输入轮胎周长计算不更新的问题。

1.0.4：
1. 关于页面增加查看更新链接（链接到这个帖子。。）；
2. 增加踏频、车速互算功能；
3. 踏频实时动态显示；
4. 支持按齿比、速比排序；
5. 增加若干轮胎、牙盘数据；
6. 踏频上限增至200
7. 感谢 @bingol  , @xuchux  , @落星  , @azuretears  ,@blocked  
ps: 
轮胎的暂时先添加一些小轮的吧，苦于手头没有数据，而且录入实在太虐了。。
分轮径胎径的方式我再考虑下怎么加入比较好，近期解决吧。
内变速近期增加，敬请期待。。


2014-05-20
1.0.3：
1. 速比计算公式改正。
2. 直径计算使用周长/PI再换算成英寸，而不是用多少多少寸，更精确。
3. 飞轮档位序号更正。
4. 在“关于”里感谢了@hachi, @heiyaa  , @楚狂声   帮忙提意见~

1.0.2：
背景图问题解决。

1.0.1：
4楼提到的不显示曲线的bug已改好~~是背景图搞得鬼。。。暂时删掉了，不影响功能，等解决了问题再把有背景图的传上来。。。
