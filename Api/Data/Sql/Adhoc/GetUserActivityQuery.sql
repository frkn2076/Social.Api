SELECT *
  FROM public.activity
 WHERE id = ANY ( SELECT activity_id
					FROM public.profile_activity
				   WHERE profileId = @id )
 LIMIT @count
OFFSET @skip