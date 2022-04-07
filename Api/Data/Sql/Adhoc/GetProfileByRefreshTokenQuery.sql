SELECT 
     ( id
     , username
     , email
     , password )
  FROM public.profile
 WHERE refreshToken = @refreshToken
