INSERT INTO public.activity
	      ( title
		  , detail
		  , location
		  , date
		  , phonenumber
		  , ownerprofileid )
	 VALUES 
	      ( @title
		  , @detail
		  , @location
		  , @date
		  , @phonenumber
		  , @ownerprofileid )
  RETURNING *
