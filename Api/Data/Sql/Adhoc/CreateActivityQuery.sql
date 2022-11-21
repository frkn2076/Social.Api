INSERT INTO public.activity
	      ( title
		  , detail
		  , location
		  , date
		  , phonenumber
		  , ownerprofileid
		  , capacity)
	 VALUES 
	      ( @title
		  , @detail
		  , @location
		  , @date
		  , @phonenumber
		  , @ownerprofileid
		  , @capacity)
  RETURNING *
