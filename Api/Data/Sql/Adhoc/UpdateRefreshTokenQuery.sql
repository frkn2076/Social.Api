UPDATE public.profile
   SET name = @name
     , surname = @surname
     , photo = @photo
 WHERE id = @id