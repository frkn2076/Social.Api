INSERT INTO public.profile_activity
	      ( activityid
		  , profileid)
	 VALUES 
	      ( @activityId
		  , @profileId)
  RETURNING *
