-- ============================================================
-- SCHEMA: PulseCategory
-- ============================================================
CREATE TABLE PulseCategory (
    Id          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    Name        NVARCHAR(100)    NOT NULL,
    CreatedAt   DATETIME2        NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_PulseCategory PRIMARY KEY (Id),
    CONSTRAINT UQ_PulseCategory_Name UNIQUE (Name)
);

-- ============================================================
-- SCHEMA: PulseEntry
-- Foreign key to PulseCategory; score constrained 1-5
-- ============================================================
CREATE TABLE PulseEntry (
    Id          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    Score       INT              NOT NULL,
    Comment     NVARCHAR(500)    NULL,
    CategoryId  UNIQUEIDENTIFIER NOT NULL,
    CreatedAt   DATETIME2        NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_PulseEntry    PRIMARY KEY (Id),
    CONSTRAINT FK_PulseEntry_Category
        FOREIGN KEY (CategoryId) REFERENCES PulseCategory(Id)
        ON DELETE RESTRICT,
    CONSTRAINT CK_PulseEntry_Score CHECK (Score BETWEEN 1 AND 5)
);

-- Performance indexes
CREATE INDEX IX_PulseEntry_CategoryId ON PulseEntry (CategoryId);
CREATE INDEX IX_PulseEntry_CreatedAt  ON PulseEntry (CreatedAt DESC);


-- ============================================================
-- QUERY 1: Overall Pulse Summary
-- Matches the shape of GET /api/pulse/summary
--
-- Score distribution uses conditional aggregation (CASE WHEN)
-- to pivot five scores into a single row. An alternative is
-- GROUP BY Score for five rows - chosen the pivot approach here
-- so consumers get a single row with named columns, matching the
-- API JSON shape more closely.
-- ============================================================
SELECT
    COUNT(*)                                                        AS [count],
    CAST(AVG(CAST(Score AS DECIMAL(10, 2))) AS DECIMAL(3, 2))      AS averageScore,
    SUM(CASE WHEN Score = 1 THEN 1 ELSE 0 END)                     AS score_1,
    SUM(CASE WHEN Score = 2 THEN 1 ELSE 0 END)                     AS score_2,
    SUM(CASE WHEN Score = 3 THEN 1 ELSE 0 END)                     AS score_3,
    SUM(CASE WHEN Score = 4 THEN 1 ELSE 0 END)                     AS score_4,
    SUM(CASE WHEN Score = 5 THEN 1 ELSE 0 END)                     AS score_5
FROM PulseEntry;


-- ============================================================
-- QUERY 2: Pulse Entry Counts by Category
-- Returns category name alongside total submission count.
-- LEFT JOIN ensures categories with zero entries are included.
-- ============================================================
SELECT
    c.Id                AS CategoryId,
    c.Name              AS CategoryName,
    COUNT(e.Id)         AS TotalCount
FROM PulseCategory c
LEFT JOIN PulseEntry e ON e.CategoryId = c.Id
GROUP BY c.Id, c.Name
ORDER BY TotalCount DESC, c.Name ASC;
