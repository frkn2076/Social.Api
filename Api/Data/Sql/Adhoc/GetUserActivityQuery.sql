SELECT *
  FROM public.activity
 WHERE id = ANY ( SELECT activityid
					FROM public.profile_activity
				   WHERE profileId = @id )