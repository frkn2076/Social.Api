SELECT *
  FROM public.profile
 WHERE id = ANY ( SELECT profileid
						  FROM public.profile_activity
						  WHERE activityId = @activityId )