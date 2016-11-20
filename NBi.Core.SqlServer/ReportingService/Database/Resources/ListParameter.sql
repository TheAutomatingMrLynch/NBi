﻿select
	Params.p.value('Name[1]','varchar(Max)') as ParameterName,
    Params.p.value('Prompt[1]','varchar(Max)') as ParameterCaption,
    Params.p.value ('Type[1]', 'VARCHAR(Max)') as DataType,
    case when Params.p.value ('Prompt[1]', 'VARCHAR(Max)')='' then 'false' else 'true' end as [Hidden]
from
	(
		select 
			ReportParams = CONVERT(XML,Parameter)
		from
			dbo.Catalog 
		where 
			Type in (2, 4) -- linked Report or Report
			--{Filters}--
	) tmp
cross apply 
	ReportParams.nodes('(Parameters/Parameter)') AS Params(p)