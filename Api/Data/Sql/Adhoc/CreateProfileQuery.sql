INSERT INTO public.profile
	      ( UserName
		  , Email
		  , password
		  , name
		  , surname
		  , Photo )
	 VALUES 
	      ( @userName
		  , @email
		  , @password
		  , @name
		  , @surname
		  , @photo )
