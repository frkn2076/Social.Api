INSERT INTO public.profile
	      ( userName
		  , email
		  , password
		  , name
		  , surname
		  , photo 
		  , refreshToken
		  , expireDate )
	 VALUES 
	      ( @userName
		  , @email
		  , @password
		  , @name
		  , @surname
		  , @photo
		  , @refreshToken
		  , @expireDate)
