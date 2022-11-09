SELECT id
     , username
     , email
     , password
     , name
     , photo
     , about
  FROM public.profile
 WHERE id = @id
