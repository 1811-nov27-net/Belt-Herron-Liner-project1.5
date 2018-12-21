create schema d20

go

create table d20.Gamer
(
	GamerId int identity not null,
	UserName nvarchar(100) not null unique,
	constraint d20_Gamer_ID primary key (GamerId)
);

create table d20.Campaign
(
	CampaignId int identity not null,
	CampaignName nvarchar(100) not null unique,
	constraint d20_Campaign_ID primary key (CampaignId)
);

create table d20.GMJunction
(
	CampaignId int not null,
	GMId int not null,
	constraint FK_GM_Gamer Foreign key (GMId) references d20.Gamer (GamerId),
	constraint FK_GM_Campaign Foreign key (CampaignId) references d20.Campaign (CampaignId)

);
