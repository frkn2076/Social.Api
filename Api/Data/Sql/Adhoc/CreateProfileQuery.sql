INSERT INTO public.profile
	      ( UserName
		  , Email
		  , Password
		  , Photo )
	 VALUES 
	      ( @userName
		  , @email
		  , @password
		  , @photo )
