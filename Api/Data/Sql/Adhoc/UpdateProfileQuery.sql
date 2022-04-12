UPDATE public.profile
   SET refreshToken = @refreshToken
     , expireDate = @expireDate
 WHERE id = @id