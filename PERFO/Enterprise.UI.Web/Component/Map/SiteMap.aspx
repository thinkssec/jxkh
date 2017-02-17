<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SiteMap.aspx.cs" Inherits="Enterprise.UI.Web.Component.Map.SiteMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>帮助说明</title>
    <link rel="stylesheet" type="text/css" media="screen, print" href="slickmap.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <link rel="Stylesheet" href="/Resources/Css/Site.css" type="text/css" />
    <style type="text/css">
        .showimg {
            width: 500px;
            margin: 10px 0;
        }
        .showimg a:link:before {
            content: "";
        }
        .showimg img {
            width: 80px;
            margin-right: 10px;
            border: solid 2px #CCC;
        }
        /*-------------------GridView------------------------*/
        .main-gridview {
            padding: 5px;
        }
        .main-gridview table {
            max-width: 100%;
            background-color: transparent;
            border-collapse: collapse;
            border-spacing: 0;
        }
        .main-gridview td {
            padding: 4px;
            border: 1px solid #dddddd;
            border-collapse: separate;
            *border-collapse: collapse;
        }
        /*表头Header*/
        .main-gridview th {
            text-align: center;
            font-weight:bold;
            background-color: #D7F1F9;
            padding: 6px;
            border: 1px solid #dddddd;
            border-collapse: separate;
            *border-collapse: collapse;
            background-image: url('/Resources/OA/site_skin/default/img/headerbg.png');
            background-repeat: repeat-x;
        }
    </style>
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        function showInfo(msg) {
            $.messager.show({
                title: '说明',
                msg: msg,
                height: '150',
                width: '280',
                showType: 'show',
                timeout: 30000
            });
        }
        function download(v) {
            switch (v) {
                case '1'://机关部门
                    window.open("/Module/Kh/FileDownload.aspx?url=" + encodeURI("/Component/Map/jietu/机关部门及负责人考核操作流程说明.doc"));
                    break;
            }
        }
    </script>
</head>
<body>
    <div class="main-gridview">
        <table style="width: 100%; text-align: center;" id="Table2">
            <tr>
                <th style="width: 10%" >序号</th>
                <th style="width: 25%" >文档名称</th>
                <th style="width: 45%" >文档说明</th>
                <th style="width: 10%" >下载</th>
            </tr>
            <tr>
                <td>1</td>
                <td><a onclick="download('1');">机关部门及负责人考核</a></td>
                <td style="text-align:left;padding:4px;">机关部门及负责人考核——用户操作及流程说明</td>
                <td><img alt="" src="/Resources/Images/ico/page_white_word.png" style="border:0;cursor:default;" onclick="download('1');"/></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>
    <hr/>
    <br/>
    <div class="sitemap">
        <h1>天然气川气东送管道分公司绩效考核系统</h1>
        <h2>Site Map Version 1.0</h2>
        <ul id="utilityNav">
            <li><a href="#" onclick="showInfo('系统目前的访问地址为‘http://10.207.33.12’');">登录</a></li>
            <li><a href="#" onclick="showInfo('可显示当前用户的账户名称、所属单位和角色等基础信息。');">当前用户</a></li>
            <li><a href="#" onclick="showInfo('可由当前用户自行操作完成登录口令的修改。缺省口令为‘a’');">修改密码</a></li>
            <li><a href="#" onclick="showInfo('可显示系统的帮助页面，方便用户查找使用。');">帮助</a></li>
        </ul>
        <ul id="primaryNav" class="col6">
            <li id="home"><a href="#C0">欢迎页</a></li>
            <li><a href="#C1">基层单位及领导班子考核</a>
                <ul>
                    <li><a href="#C1-1">基层单位年度考核指标</a></li>
                    <li><a href="#C1-2">目标值录入</a></li>
                    <li><a href="#C1-3">目标值审核</a></li>
                    <li><a href="#C1-4">完成值录入</a></li>
                    <li><a href="#C1-5">完成值审核</a></li>
                    <li><a href="#C1-6">约束和加分指标打分</a></li>
                    <li><a href="#C1-7">领导班子打分</a></li>
                    <li><a href="#C1-8">考核文件提交</a></li>
                    <li><a href="#C1-9">考核完成值审定</a></li>
                    <li><a href="#C1-10">基层单位考核结果审定</a></li>
                </ul>
            </li>
            <li><a href="#C2">机关部门及负责人考核</a>
                <ul>
                    <li><a href="#C2-1">机关部门年度考核指标</a></li>
                    <li><a href="#C2-2">机关部门自评表</a></li>
                    <li><a href="#C2-3">机关部门上级测评</a></li>
                    <li><a href="#C2-4">机关部门同级测评</a></li>
                    <li><a href="#C2-5">作风建设打分汇总</a></li>
                    <li><a href="#C2-6">连带指标打分</a></li>
                    <li><a href="#C2-7">自评文件提交</a></li>
                    <li><a href="#C2-8">机关部门考核结果审定</a></li>
                </ul>
            </li>
            <li><a href="#C3">查询统计</a>
                <ul>
                    <li><a href="#C3-1">基层单位考核结果</a></li>
                    <li><a href="#C3-2">基层单位领导班子考核结果</a></li>
                    <li><a href="#C3-3">基层单位考核明细表</a></li>
                    <li><a href="#C3-4">领导班子考核明细表</a></li>
                    <li><a href="#C3-5">机关部门考核结果</a></li>
                    <li><a href="#C3-6">部门负责人考核结果</a></li>
                    <li><a href="#C3-7">机关部门考核明细表</a></li>
                    <li><a href="#C3-8">考核结果文件</a></li>
                    <li><a href="#C3-9">历次考核对比</a></li>
                </ul>
            </li>
            <li><a href="#C4">考核管理</a>
                <ul>
                    <li><a href="#C4-1">年度绩效责任书</a></li>
                    <li><a href="#C4-2">基层单位责任书指标</a></li>
                    <li><a href="#C4-3">机关部门责任书指标</a></li>
                    <li><a href="#C4-4">发起考核</a></li>
                    <li><a href="#C4-5">基层单位指标筛选</a></li>
                    <li><a href="#C4-6">机关部门指标筛选</a></li>
                    <li><a href="#C4-7">考核参数设置</a></li>
                    <li><a href="#C4-8">数据解锁</a></li>
                    <li><a href="#C4-9">通知公告</a></li>
                    <li><a href="#C4-10">考核制度汇编</a></li>
                </ul>
            </li>
            <li><a href="#C5">指标库管理</a>
                <ul>
                    <li><a href="#C5-1">指标管理</a></li>
                    <li><a href="#C5-2">指标版本管理</a></li>
                    <li><a href="#C5-3">计算规则配置</a></li>
                    <li><a href="#C5-4">打分指标维护</a></li>
                    <li><a href="#C5-5">量化指标维护</a></li>
                    <li><a href="#C5-6">关联指标对应</a></li>
                </ul>
            </li>
            <li><a href="#C6">系统管理</a>
                <ul>
                    <li><a href="#C6-1">机构管理</a></li>
                    <li><a href="#C6-2">模块管理</a></li>
                    <li><a href="#C6-3">角色管理</a></li>
                    <li><a href="#C6-4">角色权限设置</a></li>
                    <li><a href="#C6-5">用户权限设置</a></li>
                    <li><a href="#C6-6">用户管理</a></li>
                    <li><a href="#C6-7">访问日志</a></li>
                </ul>
            </li>
        </ul>
    </div>
    <hr />
    <!--------------------------------内容---------------------------------------------->
    <div class="msg-info">
        <div class="msg-tip icon-tip"></div>
        <div>
            <span style="color: red;">提示：点击标题链接可返回到顶部!</span>
        </div>
    </div>
    <div class="help-body">
        <div class="page-header">
            <h1>系统功能介绍</h1>
        </div>
        <%-- 欢迎页面 --%>
        <div class="article">
            <p class="main"><a name="C0" class="title" onclick="window.scrollTo(0,0);return false;">欢迎页</a></p>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该页面为进入系统后的主页面。它可根据当前用户的角色和职务自动提取相应的信息进行显示，包括：当前年度内的所有考核信息、<br />
                当前的考核动态统计、通知公告、我的待办箱和考核制度汇编等功能。用户可以快速浏览到关注的信息内容。
            </p>
        </div>
        <%---------------- 基层单位及领导班子考核 ---------------------------%>
        <div class="article">
            <p class="main"><a name="C1">1. 基层单位考核</a></p>
            <a name="C1-1" class="title" onclick="window.scrollTo(0,0);return false;">1.1 基层单位年度考核指标</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可以显示各基层单位和领导班子的年度考核指标明细；只有责任书下达后，各单位才能进行查询。
            </p>
        </div>
        <%--<div class="showimg">
            <table border="0">
                <tr>
                    <td><a href="/Resources/Images/ayxBig.jpg" class="popup gallery a"><img src="/Resources/Images/ayx.jpg" alt=""/></a>&nbsp;</td>
                    <td><a href="/Resources/Images/ayxBig.jpg" class="popup gallery a"><img src="/Resources/Images/ayx.jpg" alt=""/></a>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>aaa&nbsp;</td>
                    <td>bbb&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>--%>
        <div class="article">
            <a name="C1-2" class="title" onclick="window.scrollTo(0,0);return false;">1.2 目标值录入</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                基层单位的绩效责任指标下达，同时当考核正式开始后，可由职能部门和基层单位输入各指标的具体考核值。<br />
                同时各单位还可以自行录入财务基础数据表；当指标为可将上年数据作为目标值时，还可以从财务基础数据表中自动提取。
            </p>
        </div>
        <div class="article">
            <a name="C1-3" class="title" onclick="window.scrollTo(0,0);return false;">1.3 目标值审核</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可由职能部门进行操作。基层单位的当期考核值设定后，还需要经过职能部门的审核才能正式生效。（注：由职能部门录入的目标值会自动生效）
            </p>
        </div>
        <div class="article">
            <a name="C1-4" class="title" onclick="window.scrollTo(0,0);return false;">1.4 完成值录入</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                当期考核目标值生效后，各基层单位就可以自行录入各指标的完成情况。其中包括：录入财务基础数据表，为关联指标提供基础数据；<br />
                也可直接录入关联指标数据，点击‘保存’可以暂时将完成值存储起来；点击“正式提交”后，就由相应职能部门进行审核，不能再修改录入值。
            </p>
        </div>
        <div class="article">
            <a name="C1-5" class="title" onclick="window.scrollTo(0,0);return false;">1.5 完成值审核</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                当基层单位的完成值正式提交后，可由相应的职能部门进行完成值的审核；点击‘计算指标得分’后，可以利用已配置好的计算关系式，<br />
                将完成值与考核目标值代入计算式得出各项的实际得分；
            </p>
        </div>
        <div class="article">
            <a name="C1-6" class="title" onclick="window.scrollTo(0,0);return false;">1.6 约束和加分指标打分</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可由职能部门进行操作，完成对各基层单位的约束性指标和加分指标的打分处理。
            </p>
        </div>
        <div class="article">
            <a name="C1-7" class="title" onclick="window.scrollTo(0,0);return false;">1.7 领导班子打分</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可由职能部门进行操作，负责对基层单位领导班子的约束性指标和加减分因素进行打分处理。
            </p>
        </div>
        <div class="article">
            <a name="C1-8" class="title" onclick="window.scrollTo(0,0);return false;">1.8 考核文件提交</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块由基层单位进行操作，可将本单位的考核数据文件以附件的形式上传。
            </p>
        </div>
        <div class="article">
            <a name="C1-9" class="title" onclick="window.scrollTo(0,0);return false;">1.9 考核完成值审定</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可由绩效考核办公室及绩效管理员进行操作，将各基层单位的考核结果进行最终的审定。<br />
                注：审定对象是针对已完成审核的指标。
            </p>
        </div>
        <div class="article">
            <a name="C1-10" class="title" onclick="window.scrollTo(0,0);return false;">1.10 基层单位考核结果审定</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可以实现对各基层单位及领导班子的考核成绩的统计与发布管理。可以分别统计基层单位、领导班子的指标得分情况，<br />
                并可以由绩效管理人员选择发布的结果类型，完成最终考核成绩的公布。
            </p>
        </div>

        <%---------------- 机关部门及负责人考核 ---------------------------%>
        <div class="article">
            <p class="main"><a name="C2">2. 机关部门及负责人考核</a></p>
            <a name="C2-1" class="title" onclick="window.scrollTo(0,0);return false;">2.1 机关部门年度考核指标</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                当年度绩效责任书下达后，该模块可以显示机关部门及负责人的年度考核指标。包括指标名称、分值、考核目标与评分标准等内容。
            </p>
        </div>
        <div class="article">
            <a name="C2-2" class="title" onclick="window.scrollTo(0,0);return false;">2.2 机关部门自评表</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块由当前考核期内的各机关部门进行操作，用于进行自评打分。包括打分值、完成率和打分说明等内容。
            </p>
        </div>
        <div class="article">
            <a name="C2-3" class="title" onclick="window.scrollTo(0,0);return false;">2.3 机关部门上级测评</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可由局、分公司、工程公司的领导进行打分操作，各位领导的打分值根据权重不同可折算到每个部门的最终得分上。<br />
                该模块分为两个功能区，“考核指标”为当前选定的部门的指标信息；“我测评的单位”为该领导可以打分的部门，同时还可显示打分状态。
            </p>
        </div>
        <div class="showimg">
            <table border="0">
                <tr>
                    <td><a href="/Component/Map/jietu/C2-3_1big.png" class="popup gallery a">
                        <img src="/Component/Map/jietu/C2-3_1.png" alt="" /></a>&nbsp;</td>
                    <td><a href="/Component/Map/jietu/C2-3_2big.png" class="popup gallery a">
                        <img src="/Component/Map/jietu/C2-3_2.png" alt="" /></a>&nbsp;</td>
                </tr>
                <tr>
                    <td>部门考核指标&nbsp;</td>
                    <td>我测评的单位&nbsp;</td>
                </tr>
            </table>
        </div>
        <div class="article">
            <a name="C2-4" class="title" onclick="window.scrollTo(0,0);return false;">2.4 机关部门同级测评</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可由机关部门的同级单位（各部门和基层单位）进行操作。各单位的打分根据权重不同再折算到每个部门的最终得分上。<br />
                该模块分为两个功能区，“考核指标”为当前选定的部门的指标信息；“我测评的单位”为当前用户可以打分的部门，同时还可显示打分状态。
            </p>
        </div>
        <div class="article">
            <a name="C2-5" class="title" onclick="window.scrollTo(0,0);return false;">2.5 作风建设打分汇总</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块为机关作风建设类指标的打分汇总界面，可由绩效考核办公室和管理员进行操作。可分别对每个机关部门的“互评、基层单位、领导”<br />
                打分情况进行汇总并自动排列出名次，最后再折算出具体的加分值。
            </p>
        </div>
        <div class="article">
            <a name="C2-6" class="title" onclick="window.scrollTo(0,0);return false;">2.6 连带指标打分</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块用于给机关部门负责人进行连带指标的打分操作。 
            </p>
        </div>
        <div class="article">
            <a name="C2-7" class="title" onclick="window.scrollTo(0,0);return false;">2.7 自评文件提交</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块由机部各部门进行操作，可用于被考核的机关部门以附件的形式上传自评文件。
            </p>
        </div>
        <div class="article">
            <a name="C2-8" class="title" onclick="window.scrollTo(0,0);return false;">2.8 机关部门考核结果审定</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块用于机关部门及负责人的考核结果的审定与发布管理。可以计算出机关部门与负责人考核的最终得分及排名，并可自动计算出兑现系数。
                <br />
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                本页面共有三项操作功能。“数据初始化”功能用于汇总所有参与当前考核的各机关部门及负责人信息；“统计得分”功能用于计算各部门及负责人的考核得分；
                “审定和发布结果”功能由绩效管理办公室和管理员进行操作，可以进行考核结果的最终审定与发布操作。
            </p>
        </div>

        <%---------------- 查询统计 ---------------------------%>
        <div class="article">
            <p class="main"><a name="C3">3. 查询统计</a></p>
            <a name="C3-1" class="title" onclick="window.scrollTo(0,0);return false;">3.1 基层单位考核结果</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该页面用于显示所有已发布考核结果的各基层单位的考核成绩，并可以穿透查询基层单位的考核明细表。
            </p>
        </div>
        <div class="article">
            <a name="C3-2" class="title" onclick="window.scrollTo(0,0);return false;">3.2 基层单位领导班子考核结果</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该页面用于显示所有已发布考核结果的各单位领导班子的考核成绩及排名、兑现系数，并可以穿透查询领导班子的考核明细表。
            </p>
        </div>
        <div class="article">
            <a name="C3-3" class="title" onclick="window.scrollTo(0,0);return false;">3.3 基层单位考核明细表</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该页面用于显示所有已发布考核结果的各基层单位的考核明细表。
            </p>
        </div>
        <div class="article">
            <a name="C3-4" class="title" onclick="window.scrollTo(0,0);return false;">3.4 领导班子考核明细表</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该页面用于显示所有已发布考核结果的各基层单位领导班子的考核明细表。
            </p>
        </div>
        <div class="article">
            <a name="C3-5" class="title" onclick="window.scrollTo(0,0);return false;">3.5 机关部门考核结果</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该页面用于显示所有已发布考核结果的各机关部门的考核成绩、排名。
            </p>
        </div>
        <div class="article">
            <a name="C3-6" class="title" onclick="window.scrollTo(0,0);return false;">3.6 部门负责人考核结果</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该页面用于显示所有已发布考核结果的各机关部门负责人的考核成绩、排名及兑现系数等。
            </p>
        </div>
        <div class="article">
            <a name="C3-7" class="title" onclick="window.scrollTo(0,0);return false;">3.7 机关部门考核明细表</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该页面用于显示所有已发布考核结果的各机关部门的考核结果明细表。
            </p>
        </div>
        <div class="article">
            <a name="C3-8" class="title" onclick="window.scrollTo(0,0);return false;">3.8 考核结果文件</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该页面用于显示绩效考核办公室发布的各期考核的结果文件。
            </p>
        </div>
        <div class="article">
            <a name="C3-9" class="title" onclick="window.scrollTo(0,0);return false;">3.9 历次考核对比</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该页面用于显示指定单位下的历次考核成绩的对比分析结果，也可用于显示以某考核指标为主线的各单位考核成绩对比分析。
            </p>
        </div>

        <%---------------- 考核管理 ---------------------------%>
        <div class="article">
            <p class="main"><a name="C4">4. 考核管理</a></p>
            <a name="C4-1" class="title" onclick="window.scrollTo(0,0);return false;">4.1 年度绩效责任书</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块由绩效考核办公室负责发起年度内的各单位绩效责任书制定工作。
                基层单位的责任书发起后可由负责的职能部门与基层单位共同制定年度绩效责任指标，制定完成后可由职能部门再直接下达给相应基层单位；
                机关部门的责任书发起后可由各部门自行组织制定年度绩效责任指标，制定完成后报部门领导审批，通过即可直接下达。
            </p>
        </div>
        <div class="article">
            <a name="C4-2" class="title" onclick="window.scrollTo(0,0);return false;">4.2 基层单位责任书指标</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可由基层单位和负责的职能部门共同操作，来制定本单位的绩效责任指标。该单位的责任指标根据考核采用的版本可从指标库中自动提取，
                各单位可以录入年度目标值与说明，并可配置各指标间的计算关系和修改各指标的权重与分值。
            </p>
        </div>
        <div class="article">
            <a name="C4-3" class="title" onclick="window.scrollTo(0,0);return false;">4.3 机关部门责任书指标</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可由各机关部门操作，来制定本单位的绩效责任指标。各单位责任指标根据采用的版本可从指标库中自动提取相应指标，并可配置各指标的权重。
                各部门可自行录入自己的年度指标，制定完成后报部门领导审批后才可下达。
            </p>
        </div>
        <div class="article">
            <a name="C4-4" class="title" onclick="window.scrollTo(0,0);return false;">4.4 发起考核</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块由绩效考核办公室进行操作，用于发起各类型的考核和控制考核的状态。如考核是否可查询、考核是否完成等。
            </p>
        </div>
        <div class="article">
            <a name="C4-5" class="title" onclick="window.scrollTo(0,0);return false;">4.5 基层单位指标筛选</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块由绩效考核办公室操作，用于筛选相应考核期下的各基层单位的考核指标。在该功能下还可以重新设置各指标的顺序号及权重与分值。
                在指标筛选完成后，可以发起该单位的考核；也可以删除已考核的数据；以及为考核单位追加新的指标；
            </p>
        </div>
        <div class="article">
            <a name="C4-6" class="title" onclick="window.scrollTo(0,0);return false;">4.6 机关部门指标筛选</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块由绩效考核办公室操作，用于筛选相应考核期下的各机关部门的考核指标。在该功能下还可以重新设置各指标的顺序号及权重与分值。
                在指标筛选完成后，可以发起该单位的考核；也可以删除已考核的数据；以及为考核单位追加新的指标；
            </p>
        </div>
        <div class="article">
            <a name="C4-7" class="title" onclick="window.scrollTo(0,0);return false;">4.7 考核参数设置</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可用于设置相应考核期下的一些特定参数。如：经营难度系数、成绩区间设置、排名范围设置和合并计分规则等。
            </p>
        </div>
        <div class="article">
            <a name="C4-8" class="title" onclick="window.scrollTo(0,0);return false;">4.8 数据解锁</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块用于控制各单位的锁定状态，并可解除相应单位的锁定状态。
            </p>
        </div>
        <div class="article">
            <a name="C4-9" class="title" onclick="window.scrollTo(0,0);return false;">4.9 通知公告</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块用于发布与考核相关的通知公告信息，并可记录各单位及用户的签阅数据。
            </p>
        </div>
        <div class="article">
            <a name="C4-10" class="title" onclick="window.scrollTo(0,0);return false;">4.10 考核制度汇编</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可以发布相应的考核细则信息，方便考核用户的在线查看或下载。
            </p>
        </div>

        <%---------------- 指标库管理 ---------------------------%>
        <div class="article">
            <p class="main"><a name="C5">5. 指标库管理</a></p>
            <a name="C5-1" class="title" onclick="window.scrollTo(0,0);return false;">5.1 指标管理</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可负责维护系统中所有的指标。可进行定量与定性指标的添加、修改与删除操作，并可调整指标间的顺序和控制指标的状态；
            </p>
        </div>
        <div class="article">
            <a name="C5-2" class="title" onclick="window.scrollTo(0,0);return false;">5.2 指标版本管理</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                考核指标可支持版本化。可为相同名称的指标提供不同版本下的指标含义，同时可以连带显示出当前所有已经引用了指标版本的各期考核信息。
            </p>
        </div>
        <div class="article">
            <a name="C5-3" class="title" onclick="window.scrollTo(0,0);return false;">5.3 计算规则配置</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可以为不同版本的指标分别设定计算规则，包括设定逻辑表达式、设定反射方法名称、上下限浮动值、最大最小值等。
            </p>
        </div>
        <div class="article">
            <a name="C5-4" class="title" onclick="window.scrollTo(0,0);return false;">5.4 打分指标维护</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可针对约束性指标、加减分因素指标、加分指标等定性类指标进行配置操作。包括设置每项指标的分值、打分者及机构、评分标准和计算规则等。
            </p>
        </div>
        <div class="article">
            <a name="C5-5" class="title" onclick="window.scrollTo(0,0);return false;">5.5 量化指标维护</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块为量化指标的配置操作界面。通过该模块可以实现每个量化指标的配置，包括：设置指标的说明、评分标准、计算规则、权重与分值、指标的数量单位，
                还可设置指标的对应单位、审核部门，指标完成值的计算关联表达式等。<br />
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                通过该模块，不仅可以显示每项指标的关联单位、计算规则名称，还可以进行指标间的排序操作。
            </p>
        </div>
        <div class="article">
            <a name="C5-6" class="title" onclick="window.scrollTo(0,0);return false;">5.6 关联指标对应</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块用于实现量化指标的计算关系式中的计算项与财务基础数据表中的计算项进行关联。配置完成后，对应量化指标的完成值或目标值就可以从基础数据表中直接提取出来。
            </p>
        </div>

        <%---------------- 系统管理 ---------------------------%>
        <div class="article">
            <p class="main"><a name="C6">6. 系统管理</a></p>
            <a name="C6-1" class="title" onclick="window.scrollTo(0,0);return false;">6.1 机构管理</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                实现系统内各组织机构的统一管理；支持组织机构的添加、修改、删除，同时也支持对单位的排序调整。<br />
            </p>
        </div>
        <div class="article">
            <a name="C6-2" class="title" onclick="window.scrollTo(0,0);return false;">6.2 模块管理</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                实现系统所有操作功能的维护与管理；可以对各功能模块进行添加、修改或删除，控制页面是否显示，修改访问路径等功能。<br />
            </p>
        </div>
        <div class="article">
            <a name="C6-3" class="title" onclick="window.scrollTo(0,0);return false;">6.3 角色管理</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可实现系统所有角色的定义与修改操作，包括角色的名称、图片示例等。<br />
            </p>
        </div>
        <div class="article">
            <a name="C6-4" class="title" onclick="window.scrollTo(0,0);return false;">6.4 角色权限管理</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块可以实现系统中各角色的实际功能操作权限。可以配置某一角色在各系统功能下的具体操作权限，如查看、导出、添加、修改、删除等。<br />
            </p>
        </div>
        <div class="article">
            <a name="C6-5" class="title" onclick="window.scrollTo(0,0);return false;">6.5 用户权限管理</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                用户除了拥有的角色所具有的操作权限外，还可为特定用户专门设定某一其他操作权限，即能满足权限设置的专有性。<br />
            </p>
        </div>
        <div class="article">
            <a name="C6-6" class="title" onclick="window.scrollTo(0,0);return false;">6.6 用户管理</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                可以实现对系统内所有用户的统一维护。包括新添用户、修改用户口令、职务等础信息，还可设定某一用户的分管单位信息。<br />
            </p>
        </div>
        <div class="article">
            <a name="C6-7" class="title" onclick="window.scrollTo(0,0);return false;">6.7 访问日志</a>
            <p>
                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                该模块完整记录了各个访问用户在系统中的具体操作信息。如访问时间、来路IP、用户登录名称及最后一次的访问路径等内容。<br />
            </p>
        </div>
    </div>
    <%-- 图片用脚本 --%>
    <script type="text/javascript" src="/Resources/Scripts/main.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/easy.js"></script>
</body>
</html>
