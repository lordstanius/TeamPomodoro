﻿CREATE TABLE [dbo].[Pomodoroes] (
    [PomodoroId] [uniqueidentifier] NOT NULL,
    [TaskId] [uniqueidentifier] NOT NULL,
    [StartTime] [datetime],
    [DurationInMin] [int] NOT NULL,
    [IsSuccessfull] [bit],
    CONSTRAINT [PK_dbo.Pomodoroes] PRIMARY KEY ([PomodoroId])
)
CREATE INDEX [IX_TaskId] ON [dbo].[Pomodoroes]([TaskId])
CREATE TABLE [dbo].[Tasks] (
    [TaskId] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50),
    [PomodoroCount] [int] NOT NULL,
    [UserId] [uniqueidentifier] NOT NULL,
    [ProjectId] [uniqueidentifier] NOT NULL,
    [TeamId] [uniqueidentifier],
    CONSTRAINT [PK_dbo.Tasks] PRIMARY KEY ([TaskId])
)
CREATE INDEX [IX_UserId] ON [dbo].[Tasks]([UserId])
CREATE INDEX [IX_ProjectId] ON [dbo].[Tasks]([ProjectId])
CREATE INDEX [IX_TeamId] ON [dbo].[Tasks]([TeamId])
CREATE TABLE [dbo].[Projects] (
    [ProjectId] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50),
    CONSTRAINT [PK_dbo.Projects] PRIMARY KEY ([ProjectId])
)
CREATE TABLE [dbo].[Teams] (
    [TeamId] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](20),
    CONSTRAINT [PK_dbo.Teams] PRIMARY KEY ([TeamId])
)
CREATE TABLE [dbo].[Users] (
    [UserId] [uniqueidentifier] NOT NULL,
    [UserName] [nvarchar](20),
    [Password] [nvarchar](20),
    [ShowWarningAfterPomodoroExpires] [bit] NOT NULL,
    [PomodoroDurationInMin] [int] NOT NULL,
    [TeamId] [uniqueidentifier],
    [CurrentUserTeamId] [uniqueidentifier],
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY ([UserId])
)
CREATE INDEX [IX_TeamId] ON [dbo].[Users]([TeamId])
CREATE TABLE [dbo].[UserTeams] (
    [UserTeamId] [uniqueidentifier] NOT NULL,
    [TeamId] [uniqueidentifier],
    [UserId] [uniqueidentifier] NOT NULL,
    [StartTime] [datetime],
    [StopTime] [datetime],
    CONSTRAINT [PK_dbo.UserTeams] PRIMARY KEY ([UserTeamId])
)
CREATE INDEX [IX_TeamId] ON [dbo].[UserTeams]([TeamId])
CREATE INDEX [IX_UserId] ON [dbo].[UserTeams]([UserId])
ALTER TABLE [dbo].[Pomodoroes] ADD CONSTRAINT [FK_dbo.Pomodoroes_dbo.Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([TaskId]) ON DELETE CASCADE
ALTER TABLE [dbo].[Tasks] ADD CONSTRAINT [FK_dbo.Tasks_dbo.Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([ProjectId]) ON DELETE CASCADE
ALTER TABLE [dbo].[Tasks] ADD CONSTRAINT [FK_dbo.Tasks_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE
ALTER TABLE [dbo].[Tasks] ADD CONSTRAINT [FK_dbo.Tasks_dbo.Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Teams] ([TeamId])
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_dbo.Users_dbo.Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Teams] ([TeamId])
ALTER TABLE [dbo].[UserTeams] ADD CONSTRAINT [FK_dbo.UserTeams_dbo.Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Teams] ([TeamId])
ALTER TABLE [dbo].[UserTeams] ADD CONSTRAINT [FK_dbo.UserTeams_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](150) NOT NULL,
    [ContextKey] [nvarchar](300) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
)
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201607171652426_initial', N'Model.Migrations.Configuration',  0x1F8B0800000000000400D55D6D6FE4B611FE5EA0FF41D0C7E2B2B27D28D01ABB099CB51D188DCF875B27EDB7032D71D76C246A23518E8DA2BFAC1FF293F2174A8A7AA1F822512FAB5D2340702B92C321E799E10C3973F7C7FF7E5F7EF71A85CE0B4C5214E3957BBE38731D88FD384078B77233B2FDE66FEE77DFFEF94FCB9B207A757E2EFB7D64FDE8489CAEDC6742F6979E97FACF3002E922427E12A7F1962CFC38F240107B1767677FF7CECF3D4849B89496E32CBF6498A008E63FE8CF758C7DB8271908EFE3008669F19DB66C72AACE2710C1740F7CB872F31EAE7315224027DFC070EB3A00E398004259BBFC29851B92C478B7D9D30F207C7CDB43DA6F0BC214162C5FD6DD6DB93FBB60DC7BF5C092949FA5248E7A123CFF586C87270F1FB4A96EB55D74C36EE8C69237B6EA7CD356EEE7388A8338895D479EEC721D26AC63B1A78BB2E70727FFFDA112374505FBEF83B3CE4292257085614612407B7CCE9E42E4FF03BE3DC6BF40BCC259188ADC507E685BE303FDF43989F730216F5FE056E2F12E701DAF39DE930954C33563F96A7EC810FDF327CA0B780A61257BAF95CC23487F1949624340421E29AC4B2AD78040FEBB7DE07596E402B9C3F7089783EF30F978D19B87BB7493F93E4CD32D134541EAFB380E21C0CAD84FE005EDF299359BE13A5F609837A6CF68CF356DC11ABE963B0E53D7B94DE2E84B1C166384A6AF8F20D941425988F5ED9B384B7C693D4BAF866F2BA839835D8066BDE6077389A4BE409E0081ECFF25016A05A90D779D7BF0FA23C43BF2BC72FF4A8DF62D7A8541F9A120FA1346D4E2D33124C93AE72845B88EA90D1F07566AAB93912BA63FFF0D7D3256F921886412B6BA226AC3388D2935C2A431A5465973C6F746CB56D1F695CDD160AAD1A02871B355A7C2AD66856EB3799378ABB43DECA37E63F296BE5BC210A7E58035A89B517F557642681A65C92A21759ECEBCE3110EE75AC57A9FCD9368E741AC5AEBD9A757654B9D91D1AAD7A861C75EAE229DC71EED758463AFB0A1BD8F3D8DED9D162017D3028469BE1E20DC264866ACFA683021A281EBC3021B6866834FA861A56AD0B253B78E326ADCCC76E194F59A1FA7A5D3D117A713382B8CC441B02A9B5D90A6BFC54970E87936CFF16FFF0409A6C4AFB60426A5AF72F3BA4709F375A4F0A3AF6F57909B303CB270F4A411EB2C492026A5720CF512CDA74AA7E7219F271AA764B403D661B9F42C0C75C02C2C1737201ACB552CDE60B9F2D6D196CBEE942D7B1EC7820D3D6D4D383EB81E4D603E07DFAF6C48BCB71A374C712CCE5A9D02A927F124518C85FE18B96968978DFE5CA569ECA39C0DC1D089F16E73493738703A825F7E629551213DB4A816A13DD51B3AFDCA3D7765FD78C0D73084043A573EBF805D83D407812A5CBA84C0969BCAC2D4DCD4F7A84D8EFEA24C443515B2330381704DC543751F61A2AA35C23EDA83B07D3FA4613D2E9CD88AAB49E4966BB88738A03CB6AF7FDCECD524921CBAF667E909B06A475B33243349D7109F09B22D83F059C066B8626907FE4430D36E858D988DB700BD70A65DFAE8E967009AE0A599C4AA73D96A99728B3C0BBE74B756B3804BDD011BD1EA03B25EB052573C6EE2D900C59C86763136BC877638A952D411D34122F75D9AC4CE168BF32951214C6D75AA68FDDB01A010F66FDCBC3361A27626DB44A9F12C9BD8D089B40B1F9AABA13931A24C3F234E94FD7C3F58C92D41A7589B7EFF44586906E3339E75E33026B23DE301A5C8E1E4CFA8FA59AE356E32D9213B4F43476C76FBA34C3D93ED51F6EFC4EC0E8FFAE9184247C0444901C004BEEADE2E296C8BEBB2B4B84A9685CEE86E209128A6AE535F342881B7829C2691E2125521C071D831B80830B40C94D161D7FCFC5E539D3F876EC7E0E2554919CC6DA9C560D3ECB585978808226FEE61235D41E865C8689051D87DC753B15FC94C8172F7D58C4044408F7C74341769B101D223AFBAFC962B078B4B0791EB0A712DABD7DF15746DE080758BCF10EAA24DE16F57002C305A00BC65AD9AB8F5600BCD15C2B44EE5C4EB88CB06AC523CE5C44572259E6491C2B5B47EA18650C322D89016ACE5DA22BC38E4C2B9DD6C59B8EA375B78CEC317DEF095BB003360E1420A95C16477485A75E5FA9BE9A9A45B3E37548E47D5B6F4784678F161E91952C797F760BF477827A492175F9C0DCF235F7FB3E99F6D1D711A9EDFB083B29B54CD44E204ECA0D44AA7A69CDEA22425D7808027C05EBED641A47493DD2C830F50CE267B52AAAC4ACFA01CC1FECC473513C0355E6831E496AE2762EE2B5B1AD41CC1EA508725F18310242D79DBEB38CC22DC950BDE46AD7C851029995E26CC548417469190F0D99E9694B520D2939AEC694AD9DD224DA949A5B9F424192AD1830210250C6F22CE0A8F26A7A10B8BB9D3DE1F87FA6187450D4FE91169F02FF614A49C6A9D3A144DF634CB9B039198E936A185B3FADDA5C195F939A665B78BB8B5B1DB8658F66878ADBCF201E6B3881307584FD3C839C4620FDF639990DC7B186042340E8F8D09D10E9B06D4EF5606DC4DED2F039D976D2103FDB0C39ABA3A3B53A6D3DB9C57F9970DE5ACBEF67048BA322C1B6E4A57E7FE07528B1363E872A8E3C04445939F2912D4349F944E0DB56DD5ADDE30DDEA67E34C9BDBBEAB8795FB343A3FA5C35F67193649955F67C69D1243CB5DAAD9AB585A8A999745FCDA5D93AD04B4BC8BEBD02D7A41010B66376F2981D18275586C7E0DD72182CC9F2D3BDC038CB630253C33D6BD383BBF906ABC4FA7DEDA4BD320D4C4FFBAA2EBA6BC8E500C9D61F46B06117B14425BC4EEC4461546ABE494B79F3B1CC0D795FB9F7CFCA573F7AFAF9CC407E721A192BE74CE9CFF8ECFFD0D008124FF5D53B2A959D016132006C5292AAD9F106967A85F85F300F44C537D3C1A35DC65E2C4F00B48FC679028557BFD24A72D3C1E22B9662AFA20447312A310ADD4460E62A4A2328A97663AFF301DCF4998B818A108DAD878FECAD5E36B44BF2AD121A663920ACEC36FD4C5541BA5499D9AB18470F446C9E58483364B037DA97A7022B296C582D2093ABE5070C80971047BA83261AC3C6C03CE486D186A3AA62D491BEFB39E82FC4EC3CD98CC71968BE8ECE81CA670EC586562752ADABC8561739581B53C00BFE7C2AF23D67A59A175DADAAE19CBB9ACB1F94ECAB78E55B135274A0C2F3207497BB7C6C7899762F1BCAA21D9E83DF135D599A17F193858F9CCA9A5AD0F2DAD9A4ACE0A1DFB329B7721EF09E63BA912A96356EACE8D9539ED7F2FAC9C7AA9D304B661CE737E4E9B601FA31CBB74494DBD9565A52F4C6AAD4BE2AF9D34247E6291210F85C51A1A8BBA2563D9928EB6BE144261BD0C7454CECB162DE386B2146DB993B1DA49CBB6360B5C26CBCDAF42967FD691D567D0EBC81A38AE9B4CE4F59CCF5340A5E4E1B7D70B49786D26679F5E7994AE22AAB3CEA0734B8E57FFA4D4761C7131236A9C444DA99322BB0A5D3A177FCC4AA6614B524619CB298F5CABD40F78875EDAF86AA4DED21AAB4B3D6A8ED4BC28EAA708FFA205F58F52B4AB49B07FDF0243BFE1A1547DEEF0362E1D2589A3B28B74137F0F0908A8FB729510B4053EA1CD2C0F25FF2B3E7F066146BBDC444F30B8C30F19D967842E19464FE19BB819CCE16A9B3F2FAC6AF2BC7CD8E77FDDDC144BA06C22F680F080BFCF5018547CDFAA4F1A2612CC93FB01D2EF5C96D4312470F75651FA14634B42C5F6550EE8238CF62125963EE00D78814378A39AF423DC01FFAD4C6F3313E9164473DB97D708EC12EA8E1434EAF1F427C57010BD7EFB7F8EF0A606D8650000 , N'6.1.3-40302')

