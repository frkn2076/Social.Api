SELECT id
     , username
     , email
     , password
  FROM public.profile
 WHERE id = @id
