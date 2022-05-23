SELECT *
  FROM public.profile
 WHERE profileid = ANY ( SELECT profileid
						  FROM public.profile_activity
						  WHERE activityId = @activityId )