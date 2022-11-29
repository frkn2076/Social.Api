INSERT INTO public.activity
	      ( title
		  , detail
		  , location
		  , date
		  , phonenumber
		  , ownerprofileid
		  , capacity
		  , category)
	 VALUES 
	      ( @title
		  , @detail
		  , @location
		  , @date
		  , @phonenumber
		  , @ownerprofileid
		  , @capacity
		  , @category)
  RETURNING *
