SELECT id
     , username
     , password
     , role
  FROM public.profile
 WHERE username = @userName
