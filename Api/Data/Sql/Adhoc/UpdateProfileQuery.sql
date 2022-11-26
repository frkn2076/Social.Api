UPDATE public.profile
   SET photo = COALESCE(@photo, photo)
     , name = COALESCE(@name, name)
     , about = COALESCE(@about, about)
 WHERE id = @id