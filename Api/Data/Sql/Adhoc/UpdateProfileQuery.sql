UPDATE public.profile
   SET photo = COALESCE(@photo, photo)
     , name = COALESCE(@name, name)
     , surname = COALESCE(@surname, surname)
     , about = COALESCE(@about, about)
 WHERE id = @id