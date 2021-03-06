create schema d20

go

create table d20.Gamer
(
	GamerId int identity not null primary key,
	UserName nvarchar(100) not null unique
);

create table d20.Campaign
(
	CampaignId int identity not null primary key,
	CampaignName nvarchar(100) not null unique
);

create table d20.GMJunction
(
	CampaignId int not null,
	GMId int not null,
	constraint FK_GM_Gamer Foreign key (GMId) references d20.Gamer (GamerId),
	constraint FK_GM_Campaign Foreign key (CampaignId) references d20.Campaign (CampaignId)

);

create table d20.Spells
(
	SpellId int identity not null primary key,
	SpellName nvarchar(100) not null,
	Class nvarchar(100) not null,
	SpellLevel int not null
);


create table d20.Characters
(
	CharacterId int identity not null primary key,
	GamerId int not null,
	CampaignId int,
	Race nvarchar(100) not null,
	Sex nvarchar(100) not null,
	Alignment nvarchar(100) not null,
	BAB int not null default 0,
	AC int not null default 10,
	TouchAC int not null default 10,
	FFAC int not null default 10,
	Strength int not null default 10,
	Dexterity int not null default 10,
	Stamina int not null default 10,
	Intelligence int not null default 10,
	Wisdom int not null default 10,
	Charisma int not null default 10,
	BaseFortSave int not null default 0,
	BaseReflexSave int not null default 0,
	BaseWillSave int not null default 0,
	constraint FK_Character_Gamer Foreign key (GamerId) references d20.Gamer (GamerId),
	constraint FK_Character_Campaign Foreign key (CampaignId) references d20.Campaign (CampaignId)

);

alter table d20.Characters
	add MaxHP int default 0;

alter table d20.Characters
	alter column MaxHP integer not null;

alter table d20.Characters
	add CharacterName nvarchar(100) not null;

create table d20.SpellJunction
(
	CharacterId int not null,
	SpellId int not null,
	constraint FK_SJ_Character Foreign key (CharacterId) references d20.Characters (CharacterId),
	constraint FK_SJ_Spell Foreign key (SpellId) references d20.Spells (SpellId)

);


create table d20.Skills
(
	SkillName nvarchar(100) not null,
	CharacterId int not null,
	Levels int not null default 0,
	constraint FK_Skill_Character Foreign key (CharacterId) references d20.Characters (CharacterId)
);

create table d20.Feats
(
	FeatName nvarchar(100) not null,
	CharacterId int not null,
	Copies int not null default 0,
	constraint FK_Feats_Character Foreign key (CharacterId) references d20.Characters (CharacterId)
);

create table d20.Classes
(
	ClassName nvarchar(100) not null,
	CharacterId int not null,
	Levels int not null default 0,
	constraint FK_Classes_Character Foreign key (CharacterId) references d20.Characters (CharacterId)
);

create table d20.Inventory
(
	ItemName nvarchar(100) not null,
	CharacterId int not null,
	Quantity int not null default 0,
	constraint FK_Inventory_Character Foreign key (CharacterId) references d20.Characters (CharacterId)
);

create table d20.SpellSlots
(
	CharacterId int not null,
	ClassName nvarchar(100) not null,
	Level0Slots int default 0,
	Level1Slots int default 0,
	Level2Slots int default 0,
	Level3Slots int default 0,
	Level4Slots int default 0,
	Level5Slots int default 0,
	Level6Slots int default 0,
	Level7Slots int default 0,
	Level8Slots int default 0,
	Level9Slots int default 0

);

alter table d20.SpellSlots
	add constraint FK_Slots_Character foreign key (CharacterId) references d20.Characters (CharacterId)


alter table d20.GMJunction
	add constraint PK_GM primary key (CampaignId, GMId);
alter table d20.SpellJunction
	add constraint PK_SJ primary key (CharacterId, SpellId);
alter table d20.Skills
	add constraint PK_Skills primary key (SkillName, CharacterId);
alter table d20.Feats
	add constraint PK_Feats primary key (FeatName, CharacterId);
alter table d20.Classes
	add constraint PK_Classes primary key (ClassName, CharacterId);
alter table d20.Inventory
	add constraint PK_Inventory primary key (ItemName, CharacterId);
alter table d20.SpellSlots
	add constraint PK_SpellSlots primary key (CharacterId, ClassName);


insert into d20.Gamer (UserName) values
	('No User');
select * from d20.Gamer

insert into d20.Campaign (CampaignName) values
	('No Campaign');
select * from d20.Campaign

select * from d20.Characters

insert into d20.Characters (GamerId, CampaignId, CharacterName, Race, Sex, Alignment) values
	(1, 1, 'Not a Character', 'Not a Character', 'Not a Character', 'Not a Character');

