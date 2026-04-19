CREATE TABLE [dbo].[tblEmployees] (
    [Id]          VARCHAR (22)    NOT NULL,
    [PagingOrder] INT             IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)    NOT NULL,
    [Age]         INT             NOT NULL,
    [Position]    VARCHAR (50)    NOT NULL,
    [CreatedOn]   DATETIME        DEFAULT (getdate()) NOT NULL,
    [ModifiedOn]  DATETIME        DEFAULT (getdate()) NOT NULL,
    [Salary]      DECIMAL (10, 2) NOT NULL,
    [IsArchived]  BIT             NOT NULL DEFAULT 0,
    [IsDeleted]   BIT             NOT NULL DEFAULT 0,
    [CompanyId]   VARCHAR (22)    NOT NULL,

    CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Employees_Companies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[tblCompanies] ([Id]),
    CONSTRAINT [UK_Employees_Name] UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [UK_Employees_PagingOrder] UNIQUE NONCLUSTERED ([PagingOrder]),
    INDEX [IX_Employees_CreatedOn] NONCLUSTERED (CreatedOn),
    INDEX [IX_Employees_Name] NONCLUSTERED (Name),
    INDEX [IX_Employees_IsDeleted] NONCLUSTERED ([IsDeleted] ASC)
);


