--
-- PostgreSQL database dump
--

-- Dumped from database version 15.3
-- Dumped by pg_dump version 15.2

-- Started on 2024-04-02 15:59:24

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 219 (class 1259 OID 16515)
-- Name: meetups; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.meetups (
    meetup_id integer NOT NULL,
    meetup_name character varying(50) NOT NULL,
    description character varying(1000),
    organizer_id integer,
    speaker character varying(50) NOT NULL,
    meetup_time timestamp with time zone NOT NULL,
    place_id integer NOT NULL
);


--
-- TOC entry 218 (class 1259 OID 16514)
-- Name: events_event_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.meetups ALTER COLUMN meetup_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.events_event_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 215 (class 1259 OID 16503)
-- Name: organizers; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.organizers (
    organizer_id integer NOT NULL,
    organizer_name character varying(50) NOT NULL
);


--
-- TOC entry 217 (class 1259 OID 16509)
-- Name: places; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.places (
    place_id integer NOT NULL,
    place_name character varying(50) NOT NULL
);


--
-- TOC entry 221 (class 1259 OID 16562)
-- Name: plan_steps; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.plan_steps (
    step_id integer NOT NULL,
    step_name character varying(200) NOT NULL,
    meetup_id integer NOT NULL,
    step_time timestamp with time zone NOT NULL
);


--
-- TOC entry 222 (class 1259 OID 16794)
-- Name: model_master; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.model_master AS
 SELECT m.meetup_id,
    m.meetup_name,
    m.description,
    o.organizer_id,
    o.organizer_name,
    m.speaker,
    m.meetup_time,
    p.place_id,
    p.place_name,
    ps.step_id,
    ps.step_time,
    ps.step_name
   FROM (((public.meetups m
     JOIN public.organizers o ON ((m.organizer_id = o.organizer_id)))
     LEFT JOIN public.places p ON ((m.place_id = p.place_id)))
     LEFT JOIN public.plan_steps ps ON ((ps.meetup_id = m.meetup_id)));


--
-- TOC entry 214 (class 1259 OID 16502)
-- Name: organizers_organizer_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.organizers ALTER COLUMN organizer_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.organizers_organizer_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 216 (class 1259 OID 16508)
-- Name: places_place_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.places ALTER COLUMN place_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.places_place_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 220 (class 1259 OID 16561)
-- Name: plan_steps_step_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.plan_steps ALTER COLUMN step_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.plan_steps_step_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 3108 (class 2606 OID 16521)
-- Name: meetups events_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.meetups
    ADD CONSTRAINT events_pkey PRIMARY KEY (meetup_id);


--
-- TOC entry 3104 (class 2606 OID 16507)
-- Name: organizers organizers_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.organizers
    ADD CONSTRAINT organizers_pkey PRIMARY KEY (organizer_id);


--
-- TOC entry 3106 (class 2606 OID 16513)
-- Name: places places_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.places
    ADD CONSTRAINT places_pkey PRIMARY KEY (place_id);


--
-- TOC entry 3110 (class 2606 OID 16566)
-- Name: plan_steps plan_steps_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.plan_steps
    ADD CONSTRAINT plan_steps_pkey PRIMARY KEY (step_id);


--
-- TOC entry 3111 (class 2606 OID 16522)
-- Name: meetups events_organizer_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.meetups
    ADD CONSTRAINT events_organizer_id_fkey FOREIGN KEY (organizer_id) REFERENCES public.organizers(organizer_id);


--
-- TOC entry 3112 (class 2606 OID 16527)
-- Name: meetups events_place_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.meetups
    ADD CONSTRAINT events_place_id_fkey FOREIGN KEY (place_id) REFERENCES public.places(place_id);


--
-- TOC entry 3113 (class 2606 OID 16567)
-- Name: plan_steps plan_steps_event_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.plan_steps
    ADD CONSTRAINT plan_steps_event_id_fkey FOREIGN KEY (meetup_id) REFERENCES public.meetups(meetup_id);


-- Completed on 2024-04-02 15:59:25

--
-- PostgreSQL database dump complete
--

