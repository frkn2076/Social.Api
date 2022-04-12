SELECT id
     , username
     , email
     , password
     , role
  FROM public.profile
 WHERE username = @userName
