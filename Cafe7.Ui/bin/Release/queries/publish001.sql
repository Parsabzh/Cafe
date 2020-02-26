/****** Object:  StoredProcedure [dbo].[Test]    Script Date: 8/8/2018 8:17:48 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[Test]
GO
/****** Object:  StoredProcedure [dbo].[Test]    Script Date: 8/8/2018 8:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Test]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Test] AS' 
END
GO
ALTER proc [dbo].[Test]
as
select * from Store.Product
GO
