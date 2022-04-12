INSERT INTO public.profile
	      ( userName
		  , email
		  , password
		  , name
		  , surname
		  , photo 
		  , refreshToken
		  , expireDate
		  , role )
	 VALUES 
	      ( @userName
		  , @email
		  , @password
		  , @name
		  , @surname
		  , @photo
		  , @refreshToken
		  , @expireDate
		  , @role )
  RETURNING *
