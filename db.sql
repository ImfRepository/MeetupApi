PGDMP     	    9                {            MeetupDb    15.3    15.2     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    16449    MeetupDb    DATABASE        CREATE DATABASE "MeetupDb" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Belarus.1251';
    DROP DATABASE "MeetupDb";
                postgres    false            �            1259    16515    events    TABLE     -  CREATE TABLE public.events (
    event_id integer NOT NULL,
    event_name character varying(50) NOT NULL,
    description character varying(1000),
    organizer_id integer,
    speaker character varying(50) NOT NULL,
    event_time timestamp with time zone NOT NULL,
    place_id integer NOT NULL
);
    DROP TABLE public.events;
       public         heap    postgres    false            �            1259    16514    events_event_id_seq    SEQUENCE     �   ALTER TABLE public.events ALTER COLUMN event_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.events_event_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    219            �            1259    16503 
   organizers    TABLE     y   CREATE TABLE public.organizers (
    organizer_id integer NOT NULL,
    organizer_name character varying(50) NOT NULL
);
    DROP TABLE public.organizers;
       public         heap    postgres    false            �            1259    16502    organizers_organizer_id_seq    SEQUENCE     �   ALTER TABLE public.organizers ALTER COLUMN organizer_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.organizers_organizer_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    215            �            1259    16509    places    TABLE     m   CREATE TABLE public.places (
    place_id integer NOT NULL,
    place_name character varying(50) NOT NULL
);
    DROP TABLE public.places;
       public         heap    postgres    false            �            1259    16508    places_place_id_seq    SEQUENCE     �   ALTER TABLE public.places ALTER COLUMN place_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.places_place_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    217            �            1259    16562 
   plan_steps    TABLE     �   CREATE TABLE public.plan_steps (
    step_id integer NOT NULL,
    step_name character varying(200) NOT NULL,
    event_id integer NOT NULL,
    step_time timestamp with time zone NOT NULL
);
    DROP TABLE public.plan_steps;
       public         heap    postgres    false            �            1259    16561    plan_steps_step_id_seq    SEQUENCE     �   ALTER TABLE public.plan_steps ALTER COLUMN step_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.plan_steps_step_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    221                        2606    16521    events events_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.events
    ADD CONSTRAINT events_pkey PRIMARY KEY (event_id);
 <   ALTER TABLE ONLY public.events DROP CONSTRAINT events_pkey;
       public            postgres    false    219                       2606    16507    organizers organizers_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public.organizers
    ADD CONSTRAINT organizers_pkey PRIMARY KEY (organizer_id);
 D   ALTER TABLE ONLY public.organizers DROP CONSTRAINT organizers_pkey;
       public            postgres    false    215                       2606    16513    places places_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.places
    ADD CONSTRAINT places_pkey PRIMARY KEY (place_id);
 <   ALTER TABLE ONLY public.places DROP CONSTRAINT places_pkey;
       public            postgres    false    217            "           2606    16566    plan_steps plan_steps_pkey 
   CONSTRAINT     ]   ALTER TABLE ONLY public.plan_steps
    ADD CONSTRAINT plan_steps_pkey PRIMARY KEY (step_id);
 D   ALTER TABLE ONLY public.plan_steps DROP CONSTRAINT plan_steps_pkey;
       public            postgres    false    221            #           2606    16522    events events_organizer_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.events
    ADD CONSTRAINT events_organizer_id_fkey FOREIGN KEY (organizer_id) REFERENCES public.organizers(organizer_id);
 I   ALTER TABLE ONLY public.events DROP CONSTRAINT events_organizer_id_fkey;
       public          postgres    false    219    3100    215            $           2606    16527    events events_place_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.events
    ADD CONSTRAINT events_place_id_fkey FOREIGN KEY (place_id) REFERENCES public.places(place_id);
 E   ALTER TABLE ONLY public.events DROP CONSTRAINT events_place_id_fkey;
       public          postgres    false    217    219    3102            %           2606    16567 #   plan_steps plan_steps_event_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.plan_steps
    ADD CONSTRAINT plan_steps_event_id_fkey FOREIGN KEY (event_id) REFERENCES public.events(event_id);
 M   ALTER TABLE ONLY public.plan_steps DROP CONSTRAINT plan_steps_event_id_fkey;
       public          postgres    false    221    3104    219           